using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LetsGoSEA.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace LetsGoSEA.WebSite.Services
{
    /// <summary>
    /// Mediates communication between a NeighborhoodsController and Neighborhoods Data  
    /// </summary>
    public class NeighborhoodService
    {
        // Constructor
        public NeighborhoodService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Getter: Get JSON file from wwwroot
        private IWebHostEnvironment WebHostEnvironment { get; }

        // Store the path of Neighborhoods JSON file (combine the root path, folder name, and file name)
        private string NeighborhoodFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "neighborhoods.json");

        // Generate/retrieve a list of NeighborhoodModel objects from JSON file
        public IEnumerable<NeighborhoodModel> GetNeighborhoods()
        {
            // Open Neighborhoods JSON file
            using var jsonFileReader = File.OpenText(NeighborhoodFileName);

            // Read and Deserialize JSON file into an array of NeighborhoodModel objects
            return JsonSerializer.Deserialize<NeighborhoodModel[]>(jsonFileReader.ReadToEnd(),
                // Make case insensitive
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }

        /// <summary>
        /// Returns null if passed invalid id.
        /// Returns a single neighborhood corresponding to the id
        /// </summary>
        /// <param name="id">id of the requested neighborhood</param>
        /// <returns>NeighborhoodModel of the requested neighborhood</returns>
        public NeighborhoodModel GetNeighborhoodById(int? id)
        {
            try
            {
                var data = GetNeighborhoods().Where(x => x.Id == id);
                NeighborhoodModel singleNeighborhood = data.ElementAt(0);
                return singleNeighborhood;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the id passed is invalid, we return null
                return null;
            }
            
        }

        /// <summary>
        /// Save All neighborhood data to storage
        /// </summary>
        /// <param name="neighborhoods">all the neighborhood objects to be saved</param>
        private void SaveData(IEnumerable<NeighborhoodModel> neighborhoods)
        {
            // Re-write all the neighborhood data to JSON file
            using (var outputStream = File.Create(NeighborhoodFileName))
            {
                JsonSerializer.Serialize<IEnumerable<NeighborhoodModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    neighborhoods
                );
            }
        }

        /// <summary>
        /// Remove the neighborhood record from the system
        /// </summary>
        /// <param name="id">id of the neighborhood to NOT be saved</param>
        /// <returns>the neighborhood object to be deleted</returns>
        public NeighborhoodModel DeleteData(int id)
        {
            // Get the current set
            var dataSet = GetNeighborhoods();

            // Get the record to be deleted
            var data = dataSet.FirstOrDefault(m => m.Id == id);

            // Only save the remaining records in the system
            var newDataSet = GetNeighborhoods().Where(m => m.Id != id);
            SaveData(newDataSet);

            // Return the record to be deleted
            return data;
        }
    }
}
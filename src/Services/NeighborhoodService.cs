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

        //Query to get neighborhoods data by name and return each of them
        public NeighborhoodModel GetNeighborhoodByName(string name)
        {
            var data = GetNeighborhoods().Where(x => x.Name == name);
            NeighborhoodModel singleNeighborhood = data.ElementAt(0);
            return singleNeighborhood;
        }
    }
}
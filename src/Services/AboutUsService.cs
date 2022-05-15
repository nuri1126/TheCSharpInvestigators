using LetsGoSEA.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LetsGoSEA.WebSite.Services
{
    /// <summary>
    /// Mediates communication between a AboutUsController and About Us page details   
    /// </summary>
    public class AboutUsService
    {
        // Constructor
        public AboutUsService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Getter: Get JSON file from wwwroot
        private IWebHostEnvironment WebHostEnvironment { get; }

        // Store the path of team member's JSON file (combine the root path, folder name, and file name)
        private string AboutUsFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "about_us.json");

        // Query to get all the team member objects from JSON file
        public IEnumerable<AboutUsModel> GetAboutUs()
        {
            // Open AboutUs JSON file
            using var jsonFileReader = File.OpenText(AboutUsFileName);

            // Read and Deserialize JSON file into an array of team member objects
            return JsonSerializer.Deserialize<AboutUsModel[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
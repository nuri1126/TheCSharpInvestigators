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
    public class AboutUsService
    {
        /// <summary>
        /// Mediates communication between a AboutUsController and About Us page details   
        /// </summary>
        public AboutUsService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private IWebHostEnvironment WebHostEnvironment { get; }

        // Get the Team member data's file name and create a path with combining it's root path.
        private string AboutUsFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "about_us.json");

        // Query to get Team member data and return each of them
        public IEnumerable<AboutUsModel> GetAboutUs()
        {
            using var jsonFileReader = File.OpenText(AboutUsFileName);
            return JsonSerializer.Deserialize<AboutUsModel[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
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
    public class NeighborhoodService
    {
        public NeighborhoodService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private IWebHostEnvironment WebHostEnvironment { get; }

        private string NeighborhoodFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "neighborhoods.json");

        public IEnumerable<Neighborhood> GetNeighborhoods()
        {
            using var jsonFileReader = File.OpenText(NeighborhoodFileName);
            return JsonSerializer.Deserialize<Neighborhood[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }
        
        public Neighborhood GetNeighborhoodByName(string name)
        {
            var data = GetNeighborhoods().Where(x => x.Name == name) as Neighborhood;
            Console.WriteLine(data);
            return data;
        }
    }
}
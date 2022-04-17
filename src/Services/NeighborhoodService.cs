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

            var list = JsonSerializer.Deserialize<Neighborhood[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            return list; 
        }



        /// <summary>
        ///  Returns the specified Neighborhood.
        /// </summary>
        /// <param name="name"></param>
        /// <returns> The corresponding Neighborhood object </returns>
        public Neighborhood GetNeighborhoodByName(string name)
        {
            return (Neighborhood)GetNeighborhoods().Where(x => x.Name == name); 
        }
    }

}
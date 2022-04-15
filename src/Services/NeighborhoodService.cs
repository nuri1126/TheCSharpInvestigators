using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class NeighbourhoodService
    {
        public NeighbourhoodService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private IWebHostEnvironment WebHostEnvironment { get; }

        private string NeighbourhoodFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "neighbourhoods.json");

        public IEnumerable<Neighbourhood> GetNeighbourhoods()
        {
            using var jsonFileReader = File.OpenText(NeighbourhoodFileName);
            return JsonSerializer.Deserialize<Neighbourhood[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// Index Page Model for the homepage 
    /// </summary>
    public class IndexModel : PageModel
    {
        // Logger property 
        private readonly ILogger<IndexModel> _logger;

        // Instance of Neighborhood Service for dependency injection
        private readonly NeighborhoodService _neighborhoodService;

        // Holds IEnumerable list of Neighborhoods 
        public IEnumerable<NeighborhoodModel> Neighborhoods { get; private set; }

        // Creates a logger which uses a log category equal to the name of the model.
        public IndexModel(ILogger<IndexModel> logger,
            NeighborhoodService neighborhoodService)
        {
            _logger = logger;
            _neighborhoodService = neighborhoodService;
        }

        // Returns a list of Neighborhoods to the Razor page Index.cshtml using 
        // the Neighborhood Service
        public void OnGet()
        {
            Neighborhoods = _neighborhoodService.GetNeighborhoods();
        }
    }
}
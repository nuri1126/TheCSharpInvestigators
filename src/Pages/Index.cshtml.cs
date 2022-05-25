using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// Index Page Model for the Homepage/Index Page. 
    /// </summary>
    public class IndexModel : PageModel
    {
        // Logger property.
        private readonly ILogger<IndexModel> _logger;

        // Instance of Neighborhood Service for dependency injection.
        private readonly NeighborhoodService _neighborhoodService;

        // Holds IEnumerable list of Neighborhoods.
        public IEnumerable<NeighborhoodModel> neighborhoods { get; private set; }

        /// <summary>
        /// Creates an Index logger which uses a log category equal to the name of the model.
        /// </summary>
        /// <param name="logger">An instance of the built-in ILogger service to use</param>
        /// <param name="neighborhoodService">An instance of the neighborhood service to use</param>
        public IndexModel(ILogger<IndexModel> logger,
            NeighborhoodService neighborhoodService)
        {
            _logger = logger;
            _neighborhoodService = neighborhoodService;
        }

        /// <summary>
        /// Returns a list of Neighborhoods to the Index Razor page using the Neighborhood Service.
        /// </summary>
        public void OnGet()
        {
            neighborhoods = _neighborhoodService.GetNeighborhoods();
        }
    }
}
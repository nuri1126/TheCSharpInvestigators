using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Index Page Model for the Neighborhood Razor Page: will return all the data to show
    /// </summary>
    public class IndexModel : PageModel
    {
        // Data Service
        private NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public IndexModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // Collection of the Neighborhood Data
        public IEnumerable<NeighborhoodModel> Neighborhoods { get; private set; }

        /// <summary>
        /// REST OnGet, return all data
        /// </summary>
        public void OnGet()
        {
            Neighborhoods = NeighborhoodService.GetNeighborhoods();
        }
    }
}
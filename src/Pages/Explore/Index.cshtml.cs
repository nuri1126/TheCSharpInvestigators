using System.Collections.Generic;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Index Page Model for the Explore Razor Page
    /// </summary>
    public class Index : PageModel
    {
        // Data middle tier service
        private NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="neighborhoodService">an instance of the data service to use</param>
        public Index(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // Collection of the Neighborhood Data
        public IEnumerable<NeighborhoodModel> Neighborhoods { get; private set; }

        /// <summary>
        /// REST OnGet, return all neighborhood data
        /// </summary>
        public void OnGet()
        {
            Neighborhoods = NeighborhoodService.GetNeighborhoods();
        }
    }
}
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Index Page Model for the Neighborhood Razor Page: will return all the data to show.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Data Service
        private NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public IndexModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        // Collection of the Neighborhood Data.
        public IEnumerable<NeighborhoodModel> neighborhoods { get; private set; }

        /// <summary>
        /// REST OnGet, return all data.
        /// </summary>
        public IActionResult OnGet()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            neighborhoods = neighborhoodService.GetNeighborhoods();

            return Page();
        }
    }
}
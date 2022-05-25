using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Index Page Model for the Explore Page.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Data middle tier service.
        private NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="neighborhoodService">an instance of the data service to use</param>
        public IndexModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        // Collection of the Neighborhood data.
        public IEnumerable<Models.NeighborhoodModel> neighborhoods { get; private set; }

        /// <summary>
        /// REST OnGet, return all neighborhood data.
        /// </summary>
        public void OnGet()
        {
            neighborhoods = neighborhoodService.GetNeighborhoods();
        }
    }
}
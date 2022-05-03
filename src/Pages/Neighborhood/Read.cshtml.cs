using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Read Page Model for the Read Razor Page: will return one neighborhood's data to show
    /// </summary>
    public class ReadModel : PageModel
    {
        // Data middletier
        private NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="neighborhoodService">an instance of the data service to use</param>
        public ReadModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // The data to show
        public NeighborhoodModel Neighborhood;

        /// <summary>
        /// REST Get request
        /// </summary>
        /// <param name="id">id of the neighborhood to show</param>
        public void OnGet(int id)
        {
            Neighborhood = NeighborhoodService.GetNeighborhoodById(id);
        }
    }
}
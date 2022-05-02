
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// CreateModel for adding a new Neighborhood to NeighborhoodModel and JSON file
    /// </summary>
    public class CreateModel : PageModel
    {
        // Data middle tier
        public NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Defualt Constructor
        /// </summary>
        /// <param name="neighborhoodService"></param>
        public CreateModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // The data to show
        public NeighborhoodModel Neighborhood;

        /// <summary>
        /// REST Get request
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGet()
        {
            Neighborhood = NeighborhoodService.CreateData();

            return RedirectToPage("./Update", new { Id = Neighborhood.Id });
        }
    }
}
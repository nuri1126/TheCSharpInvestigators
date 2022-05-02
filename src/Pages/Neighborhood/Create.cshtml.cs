
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Create Page Model for the Create Razor Page: adds a new Neighborhood to NeighborhoodModel and JSON file
    /// </summary>
    public class CreateModel : PageModel
    {
        // Data middle tier
        public NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Defualt Constructor
        /// </summary>
        /// <param name="neighborhoodService">An instance of the data service to use</param>
        public CreateModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // The data to show
        public NeighborhoodModel Neighborhood;

        /// <summary>
        /// REST Get request
        /// </summary>
        /// <returns>Redirected to the Update page with reference to a new NeighborhoodModel object</returns>
        public IActionResult OnGet()
        {
            // Create a new NeighborhoodModel object based on user input
            Neighborhood = NeighborhoodService.CreateData();

            // Redirect to Update page with reference to the new neighborhood
            return RedirectToPage("./Update", new { Id = Neighborhood.Id });
        }
    }
}
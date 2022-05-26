using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Read Page Model for the Read.cshtml Page: will return one neighborhood's data to show.
    /// </summary>
    public class ReadModel : PageModel
    {
        // Data middletier.
        private NeighborhoodService neighborhoodService { get; }

        // The data to show.
        public NeighborhoodModel neighborhood;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="neighborhoodService">an instance of the data service to use</param>
        public ReadModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        /// <summary>
        /// REST Get request.
        /// </summary>
        /// <param name="id">id of the neighborhood to show</param>
        public IActionResult OnGet(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Neighborhood/Index");
            }
            
            neighborhood = neighborhoodService.GetNeighborhoodById(id);

            // Redirect user to Index page when an ID out of bounds is requested
            if (neighborhood == null)
            {
                return RedirectToPage("/Neighborhood/Index");
            }

            return Page();


        }
    }
}
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Delete PageModel for the Delete.cshtml Page which manages the Delete of the data for a single record.
    /// </summary>
    public class DeleteModel : PageModel
    {
        // Data middle tier.
        public NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="neighborhoodService">An instance of the data service to be used.</param>
        public DeleteModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        // The data to show, bind to it for the post.
        [BindProperty]
        public NeighborhoodModel neighborhood { get; set; }

        /// <summary>
        /// REST Get request
        /// Loads the Data
        /// </summary>
        /// <param name="id">id of the neighborhood to delete</param>
        public IActionResult OnGet(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Neighborhood/Index");
            }
            
            neighborhood = neighborhoodService.GetNeighborhoodById(id);

            if (neighborhood == null)
            {
                return RedirectToPage("/Neighborhood/Index");
            }

            return Page();
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Neighborhood
        /// Call the data layer to Delete that data
        /// Then return to the index page
        /// </summary>
        /// <returns>redirected to the Neighborhood/Index page</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Neighborhood/Index");
            }

            neighborhoodService.DeleteData(neighborhood.id);

            return RedirectToPage("/Neighborhood/Index");
        }
    }
}
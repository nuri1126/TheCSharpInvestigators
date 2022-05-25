using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Manage the Update of the data for a single record.
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Data middletier.
        public NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public UpdateModel(NeighborhoodService neighborhoodService)
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
        /// <param name="id">id of the neighborhood to update</param>
        public void OnGet(int id)
        {
            neighborhood = neighborhoodService.GetNeighborhoodById(id);
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Neighborhood
        /// Call the data layer to Update that data
        /// Then return to the index page.
        /// </summary>
        /// <returns>reditect to Index page</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            neighborhoodService.UpdateData(neighborhood);

            return RedirectToPage("./Index");
        }
    }
}
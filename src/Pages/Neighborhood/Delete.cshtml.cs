using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Delete Page Model for the Delete Razor Page: manages the Delete of the data for a single record
    /// </summary>
    public class DeleteModel : PageModel
    {
        // Data middletier
        public NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default Construtor
        /// </summary>
        /// <param name="neighborhoodService">An instance of the data service to be used</param>
        public DeleteModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // The data to show, bind to it for the post
        [BindProperty]
        public NeighborhoodModel Neighborhood { get; set; }

        /// <summary>
        /// REST Get request
        /// Loads the Data
        /// </summary>
        /// <param name="id>id of the neighborhood to delete</param>
        public void OnGet(int id)
        {
            Neighborhood = NeighborhoodService.GetNeighborhoodById(id);
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Neighborhood
        /// Call the data layer to Delete that data
        /// Then return to the index page
        /// </summary>
        /// <returns>reditected to the Neighborhood/Index page</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NeighborhoodService.DeleteData(Neighborhood.Id);

            return RedirectToPage("./Index");
        }
    }
}
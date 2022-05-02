using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Manage the Delete of the data for a single record
    /// </summary>
    public class Delete : PageModel
    {
        // Data middletier
        public NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="neighborhoodService"></param>
        public Delete(NeighborhoodService neighborhoodService)
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
            Neighborhood = NeighborhoodService.GetNeighborhoods().FirstOrDefault(m => m.Id.Equals(id));
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
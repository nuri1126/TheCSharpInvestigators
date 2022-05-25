using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Create Page Model for the Create Razor Page: adds a new Neighborhood to NeighborhoodModel and JSON file.
    /// </summary>
    public class CreateModel : PageModel
    {
        // Data middle tier.
        public NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="neighborhoodService">An instance of the data service to use</param>
        public CreateModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        // The data to show
        public NeighborhoodModel neighborhood;

        /// <summary>
        /// REST Post request: to create a permanent neighborhood object with user input data. 
        /// </summary>
        /// <returns>Redirect to index page</returns>
        public IActionResult OnPost()
        {
            // Get user input from the form: name, image link, short description, uploaded files.
            var name = Request.Form["Neighborhood.Name"];
            var imageURLs = Request.Form["Neighborhood.Image"];
            var shortDesc = Request.Form["Neighborhood.ShortDesc"];
            var imageFiles = Request.Form.Files;

            /*
             * Create a new Neighborhood Model object WITH user input
             * This Neighborhood object is different from the object created in OnGet(). This 
             * object will store user input and eventually convert them to JSON.
             */
            neighborhood = neighborhoodService.AddData(name, imageURLs, shortDesc, imageFiles);

            // Redirect to Update page with reference to the new neighborhood
            return RedirectToPage("./Index");
        }
    }
}
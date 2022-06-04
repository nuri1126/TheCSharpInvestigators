using System;
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
        /// Call the data layer to Update that data
        /// Then return to the index page.
        /// </summary>
        /// <returns>redirect to Index page</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Neighborhood/Index");
            }

            // Get user selected images to delete from the form.
            var deleteImageIds = Request.Form["DeleteFile"].ToArray();

            // Get user uploaded image files from the form.
            var imagesToUpload = Request.Form.Files;

            // If neighborhood is not null, update neighborhood with user entered data. 
            if (neighborhood != null)
            {
                neighborhood = neighborhoodService.UpdateData(neighborhood);
            }

            // If user selected images to delete, delete those images from current neighborhood.
            if (neighborhood != null && deleteImageIds.Length != 0)
            {
                neighborhood = neighborhoodService.DeleteUploadedImage(neighborhood, deleteImageIds);
            }

            // If user has uploaded new images, upload those images to current neighborhood. 
            if (neighborhood != null && imagesToUpload.Count != 0)
            {
                neighborhoodService.UploadImageIfAvailable(neighborhood, imagesToUpload);
            }

            return RedirectToPage("/Neighborhood/Index");
        }
    }
}
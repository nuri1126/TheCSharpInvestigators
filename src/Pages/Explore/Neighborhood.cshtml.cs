using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Neighborhood Page Model.
    /// </summary>
    public class NeighborhoodModel : PageModel
    {
        // Data middle tier service.
        private NeighborhoodService neighborhoodService { get; }

        // Initialize current average rating to be displayed to user.
        public int avgRating = 0;

        // Initialize current vote count to be displayed to user.
        public int voteCount = 0;

        // Denote "votes" or "vote" to be displayed to user.
        public string voteLabel;

        // Set comment section max character length.
        public int commentMaxChar = 300;

        // Store the list of all neighborhood images.
        public List<string> allImages;

        // Holds the users input from the comment form.
        [BindProperty]
        public string newCommentText { get; set; } = "";

        /// <summary>
        /// Neighborhood default constructor.
        /// </summary>
        /// <param name="neighborhoodService">An instance of data service to use.</param>
        public NeighborhoodModel(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }

        [BindProperty]
        // Current Neighborhood to be displayed to the user.
        public Models.NeighborhoodModel currentNeighborhood { get; set; }

        [BindProperty]
        // User input rating.
        public int rating { get; set; } = 0;

        /// <summary>
        /// Get current average rating and vote count for the current neighborhood to display to user.
        /// </summary>
        private void GetCurrentRating()
        {
            if (currentNeighborhood.ratings == null)
            {
                avgRating = 0;
                voteCount = 0;
            }
            else
            {
                voteCount = currentNeighborhood.ratings.Count();
                voteLabel = voteCount > 1 ? "Votes" : "Vote";
                avgRating = currentNeighborhood.ratings.Sum() / voteCount;
            }

            System.Console.WriteLine($"Current rating for {currentNeighborhood.id}: {avgRating}");
        }

        /// <summary>
        /// This method parses the id from the url and checks if the id is valid.
        /// If invalid id is provided, 404 Not found Page is returned
        /// Else, we return the neighborhood page associated with that id 
        /// </summary>
        /// <param name="id">id of the neighborhood</param>
        /// <returns>View of the requested neighborhood</returns>
        public IActionResult OnGet(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Index");
            }

            currentNeighborhood = neighborhoodService.GetNeighborhoodById(id);

            // If invalid id is passed, it results in CurrentNeighborhood to be null
            // User is redirected to the Explore Page
            if (currentNeighborhood == null)
            {
                return RedirectToPage("./Index");
            }

            // Update current rating 
            GetCurrentRating();

            // Set neighborhoodImage list
            allImages = neighborhoodService.GetAllImages(currentNeighborhood);

            return Page();
        }

        /// <summary>
        /// REST Post method: when user clicks on rating star, this method updates the rating for the current neighborhood and 
        /// calls GetCurrentRating() to display the new average rating and vote count to user. When user submits comment, 
        /// this method updates the neighborhood's Comment list. 
        /// </summary>
        /// <param name="id">the id of the current neighborhood</param>
        /// <param name="CommentId"></param>
        public IActionResult OnPost(int id, string CommentId)
        {
            // Assign the user's selected neighborhood to the CurrentNeighborhood var
            currentNeighborhood = neighborhoodService.GetNeighborhoodById(id);


            if (rating != 0)
            {
                // Add Rating to neighborhood model 
                neighborhoodService.AddRating(currentNeighborhood, rating);

                //return Redirect("/Explore/Neighborhood/" + id.ToString());
            }

            if (newCommentText != "")
            {
                // Add Comment to neighborhood model
                neighborhoodService.AddComment(currentNeighborhood, newCommentText);

                // Redirect to comment section of the page
                //return Redirect("/Explore/Neighborhood/" + id.ToString() + "/#commentAnchor");
            }

            if (CommentId != "")
            {
                neighborhoodService.DeleteComment(currentNeighborhood, CommentId);
            }

            //Update current rating
            GetCurrentRating();

            return Redirect("/Explore/Neighborhood/" + id.ToString()+"#comments");
        }
    }
}
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Neighborhood Page Model for the Neighborhood Razor Page
    /// </summary>
    public class NeighborhoodModel : PageModel
    {
        // Data middle tier service
        private NeighborhoodService NeighborhoodService { get; }

        public int avgRating = 0;          // initialize current average rating to be displayed to user 
        public int voteCount = 0;          // initialize current vote count to be displayed to user
        public string voteLabel;           // denote "votes" or "vote" to be displayed to user


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public NeighborhoodModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        [BindProperty]
        // Current Neighborhood to be displayed to the user
        public Models.NeighborhoodModel CurrentNeighborhood { get; set; }

        [BindProperty]
        // User input rating 
        public int Rating { get; set; } = 0;

        /// <summary>
        /// Get current average rating and vote count for the current neighborhood to display to user
        /// </summary>
        private void GetCurrentRating()
        {
            if (CurrentNeighborhood.Ratings == null)
            {
                avgRating = 0;
                voteCount = 0;
            }
            else
            {
                voteCount = CurrentNeighborhood.Ratings.Count();
                voteLabel = voteCount > 1 ? "Votes" : "Vote";
                avgRating = CurrentNeighborhood.Ratings.Sum() / voteCount;
            }

            System.Console.WriteLine($"Current rating for {CurrentNeighborhood.Id}: {avgRating}");
        }

        // Holds the users input from the comment form
        [BindProperty]
        public string NewCommentText { get; set; } = "";

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

            CurrentNeighborhood = NeighborhoodService.GetNeighborhoodById(id);

            // If invalid id is passed, it results in CurrentNeighborhood to be null
            // User is redirected to the Explore Page
            if (CurrentNeighborhood == null)
            {
                return RedirectToPage("./Index");
            }

            // Update current rating 
            GetCurrentRating();

            return Page();
        }

        /// <summary>
        /// REST Post method: when user clicks on rating star, this method updates the rating for the current neighborhood and 
        /// calls GetCurrentRating() to display the new average rating and vote count to user. When user submits comment, 
        /// this method updates the neighborhood's Comment list. 
        /// </summary>
        /// <param name="id">the id of the current neighborhood</param>
        public IActionResult OnPost(int id)
        {
            // Assign the user's selected neighborhood to the CurrentNeighborhood var
            CurrentNeighborhood = NeighborhoodService.GetNeighborhoodById(id);


            if (Rating != 0)
            {
                // Add Rating to neighborhood model 
                NeighborhoodService.AddRating(CurrentNeighborhood, Rating);

                //return Redirect("/Explore/Neighborhood/" + id.ToString());
            }

            if (NewCommentText != "")
            {
                // Add Comment to neighborhood model
                NeighborhoodService.AddComment(CurrentNeighborhood, NewCommentText);

                // Redirect to comment section of the page
                //return Redirect("/Explore/Neighborhood/" + id.ToString() + "/#commentAnchor");
            }

            //Update current rating
            GetCurrentRating();

            return Page();
            //return Redirect("/Explore/Neighborhood/" + id.ToString());


        }
    }
}
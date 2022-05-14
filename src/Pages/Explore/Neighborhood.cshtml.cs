using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Neighborhood Page Model for the Neighborhood Razor Page
    /// </summary>
    public class NeighborhoodModel : PageModel
    {
        // Data middle tier service
        private NeighborhoodService NeighborhoodService { get; }
        // API Key for Google Maps 
        private string API_KEY = "AIzaSyCREdLVae8DOZP70uabA9l-VRSe83QwcYs";

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
        public int Rating { get; set; }

        /// <summary>
        /// Getting function to retrieve the API key
        /// </summary>
        /// <returns>Google Maps API Key</returns>
        public string GetKey()
        {
            return API_KEY;
        }

        /// <summary>
        /// Makes an API call to Redfin WalkScore API to get 
        /// Walk Score, Transit Score, Bike Score of a single neighborhood
        /// </summary>
        /// <param name="neighborhood">A NeighborhoodModel of a single neighborhood</param>
        /// <returns>An object with the response from the API</returns>
        public async Task<NeighborhoodCharacteristics> GetWalkScore(Models.NeighborhoodModel neighborhood)
        {
            const string apiKey = "c6d78988747a933bd54771e51a75dd26";

            // API URL
            string url =
                $"https://api.walkscore.com/score?format=json&address={neighborhood.Address}&lat={neighborhood.Latitude}&lon={neighborhood.Longitude}&bike=1&wsapikey={apiKey}";


            using HttpClient client = new HttpClient();
            NeighborhoodCharacteristics characteristics = new NeighborhoodCharacteristics();

            // API Call is made and we save the response
            HttpResponseMessage response = await client.GetAsync(url);

            // Check if the response has a success code
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response and map it to a data model
                characteristics = JsonConvert.DeserializeObject<NeighborhoodCharacteristics>(res);
            }


            return characteristics;
        }

        /// <summary>
        /// A class to store the response from the API with the
        /// required fields
        /// </summary>
        public class NeighborhoodCharacteristics
        {
            public int WalkScore { get; set; }
            public string Description { get; set; }
            public Dictionary<string, string> Bike { get; set; }
        }


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

            // Get the Walk Score, Transit Score, Bike score of the neighborhood if available
            var walkScores = GetWalkScore(CurrentNeighborhood).Result;

            // Validating the value of WalkScore
            if (walkScores.WalkScore > 0)
            {
                // Setting the values to the Model
                CurrentNeighborhood.WalkScore = walkScores.WalkScore;
                CurrentNeighborhood.WalkScoreDescription = walkScores.Description;
            }

            // Validating we got a valid response for Bike Score
            if (walkScores.Bike["score"] != null)
            {
                // Setting the values to the Model
                CurrentNeighborhood.BikeScore = walkScores.Bike["score"][0];
                CurrentNeighborhood.BikeScoreDescription = walkScores.Bike["description"];
            }

            GetCurrentRating();
            return Page();
        }

        /// <summary>
        /// REST Post method: when user clicks on rating star, this method updates the rating for the current neighborhood and 
        /// calls GetCurrentRating() to display the new average rating and vote count to user. 
        /// </summary>
        /// <param name="id">the id of the current neighborhood</param>
        public void OnPost(int id)
        {
            CurrentNeighborhood = NeighborhoodService.GetNeighborhoodById(id);
            NeighborhoodService.AddRating(CurrentNeighborhood, Rating);
            GetCurrentRating();
        }
    }
}

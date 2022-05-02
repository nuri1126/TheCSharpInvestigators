using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LetsGoSEA.WebSite.Pages.Explore
{
    /// <summary>
    /// Neighborhood Page Model for the Neighborhood Razor Page
    /// </summary>
    public class Neighborhood : PageModel
    {
        // Data middle tier service
        private NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public Neighborhood(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        [BindProperty]
        // Current Neighborhood to be displayed to the user
        public NeighborhoodModel CurrentNeighborhood { get; private set; }

        /// <summary>
        /// Makes an API call to Redfin WalkScore API to get 
        /// Walk Score, Transit Score, Bike Score of a single neighborhood
        /// </summary>
        /// <param name="neighborhood">A NeighborhoodModel of a single neighborhood</param>
        /// <returns>An object with the response from the API</returns>
        public async Task<NeighborhoodCharacteristics> GetWalkScore(NeighborhoodModel neighborhood)
        {
            const string apiKey = "c6d78988747a933bd54771e51a75dd26";

            // API URL
            string url =
                $"https://api.walkscore.com/score?format=json&address={neighborhood.Address}&lat={neighborhood.Latitude}&lon={neighborhood.Longitude}&transit=1&bike=1&wsapikey={apiKey}";


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
            public Dictionary<string, string> Transit { get; set; }
            public Dictionary<string, string> Bike { get; set; }
        }

        /// <summary>
        /// This method parses the id from the url and checks if the id is valid.
        /// If invalid id is provided, 404 Not found Page is returned
        /// Else, we return the neighborhood page associated with that id 
        /// </summary>
        /// <param name="id">id of the neighborhood</param>
        /// <returns>View of the requested neighborhood</returns>
        public IActionResult OnGet(int? id)
        {
            // If no id is passed, NotFound status message is returned
            if (id == null)
            {
                return NotFound();
            }

            CurrentNeighborhood = NeighborhoodService.GetNeighborhoodById(id);

            // If invalid id is passed, it results in CurrentNeighborhood to be null
            // NotFound status message is returned to safe guard the website
            if (CurrentNeighborhood == null)
            {
                return NotFound();
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

            // Validating we got a valid response for Transit Score
            if (walkScores.Transit["score"] != null)
            {
                // Setting the values to the Model
                CurrentNeighborhood.TransitScore = walkScores.Transit["score"][0];
                CurrentNeighborhood.TransitScoreDescription = walkScores.Transit["description"];
            }

            // Validating we got a valid response for Bike Score
            if (walkScores.Bike["score"] != null)
            {
                // Setting the values to the Model
                CurrentNeighborhood.BikeScore = walkScores.Bike["score"][0];
                CurrentNeighborhood.BikeScoreDescription = walkScores.Bike["description"];
            }

            return Page();
        }
    }
}
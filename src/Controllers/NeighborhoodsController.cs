using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using LetsGoSEA.WebSite.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using LetsGoSEA.WebSite.Data;
using System.Net.Http;
using Newtonsoft.Json;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// Neighborhoods Controller 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodsController : Controller
    {
        // Holds DB context string connection
        private readonly LetsGoSEAWebSiteContext _context;

        // Seed controller with service and DB context
        public NeighborhoodsController(NeighborhoodService neighborhoodService, LetsGoSEAWebSiteContext context)
        {
            //TODO: Add null context check here

            this.NeighborhoodService = neighborhoodService;
            this._context = context;
        }

        // Controller's retrievable service property used for dependency injection
        public NeighborhoodService NeighborhoodService { get; }

        // Displays View of all neighborhoods in card layout
        [HttpGet("/Neighborhoods")]
        public IActionResult Explore()
        {
            return View("Explore"); 
        }

        // Displays a View of a selected neighborhood in its own tab
        [HttpGet("/Neighborhoods/{name}")]
        public IActionResult GetNeighborhood(string name)
        {
            // Renders a ViewResult object of the specific neighborhood
            var viewModel = NeighborhoodService.GetNeighborhoodByName(name);

            // Get the Walk Score, Transit Score, Bike score of the neighborhood if available
            var WalkScores = GetWalkScore(viewModel).Result;
            
            // Validating the value of WalkScore
            if (WalkScores.WalkScore > 0)
            {
                // Setting the values to the Model
                viewModel.WalkScore = WalkScores.WalkScore;
                viewModel.WalkScoreDescription = WalkScores.Description;
            }

            // Validating we got a valid response for Transit Score
            if (WalkScores.Transit["score"] != null)
            {
                // Setting the values to the Model
                viewModel.TransitScore = WalkScores.Transit["score"][0];
                viewModel.TransitScoreDescription = WalkScores.Transit["description"];

            }

            // Validating we got a valid response for Bike Score
            if (WalkScores.Bike["score"] != null)
            {
                // Setting the values to the Model
                viewModel.BikeScore = WalkScores.Bike["score"][0];
                viewModel.BikeScoreDescription = WalkScores.Bike["description"];
            }
            
            return View(viewModel);
        }


        /// <summary>
        /// Makes an API call to Redfin WalkScore API to get 
        /// Walk Score, Transit Score, Bike Score of a single neighborhood
        /// </summary>
        /// <param name="neighborhood">A NeighborhoodModel of a single neighborhood</param>
        /// <returns>An object with the response from the API</returns>
        public async Task<NeighborhoodCharacteristics> GetWalkScore(NeighborhoodModel neighborhood)
        {
            const string API_KEY = "c6d78988747a933bd54771e51a75dd26";

            // API URL
            string url = $"https://api.walkscore.com/score?format=json&address={neighborhood.Address}&lat={neighborhood.Latitude}&lon={neighborhood.Longitude}&transit=1&bike=1&wsapikey={API_KEY}";
            

            using (HttpClient client = new HttpClient())
            {
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

            
        }

        // Displays a view of the Create page
        //[HttpGet("/Neighborhoods/Create")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id,name,image,city,state,shortDesc")] NeighborhoodModel neighborhood)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(neighborhood);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(neighborhood));
        //    }
        //    return View(neighborhood);
        //}

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

}
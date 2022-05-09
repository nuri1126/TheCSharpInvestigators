using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using System;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    /// <summary>
    /// Create Page Model for the Create Razor Page: adds a new Neighborhood to NeighborhoodModel and JSON file
    /// </summary>
    public class CreateModel : PageModel
    {
        // Data middle tier
        public NeighborhoodService NeighborhoodService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="neighborhoodService">An instance of the data service to use</param>
        public CreateModel(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        // The data to show
        public NeighborhoodModel Neighborhood;

        public void OnGet()
        {
            // Create a new NeighborhoodModel object
            // The sole purpose is to show the autopopulated ID, Seattle, and WA fields on the browser
            Neighborhood = NeighborhoodService.CreateID();
        }
       
    }
}
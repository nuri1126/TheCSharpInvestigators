using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// Neighborhoods Page Controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodsController : Controller
    {
        public NeighborhoodsController(NeighborhoodService neighborhoodService)
        {
            this.NeighborhoodService = neighborhoodService;
        }
        
        public NeighborhoodService NeighborhoodService { get; }

        /// Route: /neighborhoods
        /// Shows all neighborhoods in card layout
        public IActionResult Index()
        {
            return View();
        }

        /// Route: /neighborhoods/{name}
        // Returns A View with all the neighborhoods
        [HttpGet("{name}")]
        public IActionResult GetNeighborhood(string name)
        {
            // Get neighborhoods' data by name and return each of them to it's detail pages(view)
            var viewModel = NeighborhoodService.GetNeighborhoodByName(name);
            return View(viewModel);
        }


    }
}
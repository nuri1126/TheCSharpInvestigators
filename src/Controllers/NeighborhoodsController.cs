using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodsController : Controller
    {
        public NeighborhoodsController(NeighborhoodService neighborhoodService)
        {
            this.NeighborhoodService = neighborhoodService;
        }
        
        public NeighborhoodService NeighborhoodService { get; }

        // [HttpGet]
        // public IEnumerable<Neighborhood> Get()
        // {
        //     return NeighborhoodService.GetNeighborhoods();
        // }

        /// <summary>
        /// Route: /neighborhoods
        /// Shows all neighborhoods in card layout
        /// </summary>
        /// <returns>A View with all the neighborhoods</returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{name}")]
        public IActionResult GetNeighborhood(string name)
        {
            var viewModel = NeighborhoodService.GetNeighborhoodByName(name);
            return View(viewModel);
        }


    }
}
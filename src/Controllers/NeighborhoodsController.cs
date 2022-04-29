using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using LetsGoSEA.WebSite.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using LetsGoSEA.WebSite.Data;

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
            return View(); 
        }

        // Displays a View of a selected neighborhood in its own tab
        [HttpGet("/Neighborhoods/{name}")]
        public IActionResult GetNeighborhood(string name)
        {
            // Renders a ViewResult object of the specific neighborhood
            var viewModel = NeighborhoodService.GetNeighborhoodByName(name);
            return View(viewModel);
        }

        // Displays a view of the Create page
        [HttpGet("/Neighborhoods/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,image,city,state,shortDesc")] NeighborhoodModel neighborhood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neighborhood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(neighborhood));
            }
            return View(neighborhood);
        }

    }
}
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// Neighborhoods Controller 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodsController : Controller
    {
        // Seed controller with service 
        public NeighborhoodsController(NeighborhoodService neighborhoodService)
        {
            // Assign passed in service to controller prop 
            this.NeighborhoodService = neighborhoodService;
        }

        // Controller's retrievable service property used for dependency injection
        public NeighborhoodService NeighborhoodService { get; }

        // Route: /neighborhoods
        // Displays View of all neighborhoods in card layout
        public IActionResult Index()
        {
            return View(); // "Views/Neighborhoods/Index.cshtml" 
        }

        // Route: /neighborhoods/{name}
        // Displays a View of a selected neighborhood in its own tab
        [HttpGet("{name}")]
        public IActionResult GetNeighborhood(string name)
        {
            // Renders a ViewResult object of the specific neighborhood
            var viewModel = NeighborhoodService.GetNeighborhoodByName(name);
            return View(viewModel); // "Views/GetNeighborhoods.cshtml"
        }


    }
}
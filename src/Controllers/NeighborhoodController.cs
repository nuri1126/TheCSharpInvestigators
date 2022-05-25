using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// Neighborhoods Controller for Neighborhoods Pages and Model.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodController : Controller
    {

        // Controller's retrievable service property used for dependency injection
        public NeighborhoodService neighborhoodService { get; }

        /// <summary>
        /// Seed controller with neighborhood data service 
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public NeighborhoodController(NeighborhoodService neighborhoodService)
        {
            this.neighborhoodService = neighborhoodService;
        }
    }
}
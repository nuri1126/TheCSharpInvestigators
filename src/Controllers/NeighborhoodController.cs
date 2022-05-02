using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using LetsGoSEA.WebSite.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// Neighborhoods Controller 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NeighborhoodController : Controller
    {
        /// <summary>
        /// Seed controller with neighborhood data service 
        /// </summary>
        /// <param name="neighborhoodService">an instance of data service to use</param>
        public NeighborhoodController(NeighborhoodService neighborhoodService)
        {
            this.NeighborhoodService = neighborhoodService;
        }

        // Controller's retrievable service property used for dependency injection
        private NeighborhoodService NeighborhoodService { get; }
    }
}
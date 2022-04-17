using System.Collections;
using System.Collections.Generic;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NeighborhoodsController : Controller
    {
        public NeighborhoodsController(NeighborhoodService neighborhoodService)
        {
            this.NeighborhoodService = neighborhoodService;
        }
        
        public NeighborhoodService NeighborhoodService { get; }

        [HttpGet]
        public IEnumerable<Neighborhood> Get()
        {
            return NeighborhoodService.GetNeighborhoods();
        }

       /// <summary>
       /// Returns a new view of the Neighborhood matching input name. 
       /// </summary>
       /// <param name="name">Name of Neighborhood</param>
       /// <returns></returns>
        public ViewResult GetNeighborhood(string name) 
        {
           return View(NeighborhoodService.GetNeighborhoodByName(name));
        }
        
    }
}
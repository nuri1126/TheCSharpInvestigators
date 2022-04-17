using System.Collections;
using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
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

       
        public ViewResult GetNeighborhood(string name) 
        {
           return View(NeighborhoodService.GetNeighborhoodByName(name));
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NeighbourhoodsController : ControllerBase
    {
        public NeighbourhoodsController(NeighbourhoodService neighbourhoodService)
        {
            this.NeighbourhoodService = neighbourhoodService;
        }
        
        public NeighbourhoodService NeighbourhoodService { get; }

        [HttpGet]
        public IEnumerable<Neighbourhood> Get()
        {
            return NeighbourhoodService.GetNeighbourhoods();
        }
    }
}
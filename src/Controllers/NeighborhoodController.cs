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
        // Seed controller with service and DB context
        public NeighborhoodController(NeighborhoodService neighborhoodService)
        {
            //TODO: Add null context check here

            this.NeighborhoodService = neighborhoodService;
        }

        // Controller's retrievable service property used for dependency injection
        private NeighborhoodService NeighborhoodService { get; }


        // Displays a view of the Create page
        //[HttpGet("/Neighborhoods/Create")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("id,name,image,city,state,shortDesc")] NeighborhoodModel neighborhood)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(neighborhood);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(neighborhood));
        //    }
        //    return View(neighborhood);
        //}
    }
}
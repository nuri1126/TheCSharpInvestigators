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
    /// About Us Page Controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AboutUsController : Controller
    {
        public AboutUsController(AboutUsService aboutUsService)
        {
            this.aboutUsService = aboutUsService;
        }

        public AboutUsService aboutUsService { get; }

        // Route: /AboutUs
        // Returns a View with all the team member info
        public IActionResult Index()
        {
            return View();
        }


    }
}
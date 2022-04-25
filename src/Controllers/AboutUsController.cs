using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Controllers
{
    /// <summary>
    /// About Us Controller routes a View to information with developer bios. 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AboutUsController : Controller
    {
        // Seed controller with service 
        public AboutUsController(AboutUsService aboutUsService)
        {
            // Assign passed in service to controller prop 
            this.aboutUsService = aboutUsService; 
        }

        // Controller's retrievable service property used for dependency injection
        public AboutUsService aboutUsService { get; }

        // Route: /AboutUs
        // Renders a ViewResult object with developer bios
        public IActionResult Index()
        {
            return View(); // "View/AboutUs/Index.cshtml" 
        }
    }
}
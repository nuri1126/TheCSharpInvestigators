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
            this.aboutUsService = aboutUsService; // Assign passed in service to controller prop 
        }

        // Controller's retrievable service property used for dependency injection
        public AboutUsService aboutUsService { get; }

        // Route: /AboutUs
        // Returns a View with developer bios
        public IActionResult Index()
        {
            return View(); // "AboutUs/Index.cshtml" 
        }
    }
}
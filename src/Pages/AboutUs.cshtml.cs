using System.Collections.Generic;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// AboutUs Page Model for the AboutUs Razor Page
    /// </summary>
    public class AboutUsModel : PageModel
    {
        // Data service used to get team member data
        private AboutUsService AboutUsService { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="aboutUsService">An instance of the data service to use</param>
        public AboutUsModel(AboutUsService aboutUsService)
        {
            AboutUsService = aboutUsService;
        }

        // Holds IEnumerable list of team member objects
        public IEnumerable<Models.AboutUsModel> Members { get; private set; }

        /// <summary>
        /// Returns a list of team member objects to the AboutUs Razor Page
        /// </summary>
        public void OnGet()
        {
            Members = AboutUsService.GetAboutUs();
        }
    }
}
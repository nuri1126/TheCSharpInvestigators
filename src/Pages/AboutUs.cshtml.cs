using System.Collections.Generic;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// AboutUs Page Model for the AboutUs Razor Page
    /// </summary>
    public class AboutUs : PageModel
    {
        // Data service used to get team member data
        public AboutUsService AboutUsService { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="aboutUsService">An instance of the data service to use</param>
        public AboutUs(AboutUsService aboutUsService)
        {
            AboutUsService = aboutUsService;
        }

        // Holds IEnumerable list of team member objects
        public IEnumerable<AboutUsModel> Members;

        /// <summary>
        /// Returns a list of team member objects to the AboutUs Razor Page
        /// </summary>
        public void OnGet()
        {
            Members = AboutUsService.GetAboutUs();
        }
    }
}
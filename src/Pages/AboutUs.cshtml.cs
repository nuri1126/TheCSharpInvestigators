using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// AboutUs Page Model for the AboutUs.cshtml Page
    /// </summary>
    public class AboutUsModel : PageModel
    {
        // Data service used to get team member data
        private AboutUsService aboutUsService { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="aboutUsService">An instance of the data service to use</param>
        public AboutUsModel(AboutUsService aboutUsService)
        {
            this.aboutUsService = aboutUsService;
        }

        // Holds IEnumerable list of team member objects
        public IEnumerable<Models.AboutUsModel> members { get; private set; }

        /// <summary>
        /// Returns a list of team member objects to the AboutUs Razor Page
        /// </summary>
        public void OnGet()
        {
            members = aboutUsService.GetAboutUs();
        }
    }
}
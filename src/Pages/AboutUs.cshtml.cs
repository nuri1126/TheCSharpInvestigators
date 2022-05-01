using System.Collections.Generic;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages
{
    public class AboutUs : PageModel
    {
        
        public AboutUsService AboutUsService { get; }

        public AboutUs(AboutUsService aboutUsService)
        {
            AboutUsService = aboutUsService;
        }

        public IEnumerable<AboutUsModel> Members;

        public void OnGet()
        {
            Members = AboutUsService.GetAboutUs();
        }
    }
}
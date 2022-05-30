using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// Privacy Page Model for the Privacy Page.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        /// <summary>
        /// Default OnGet.
        /// </summary>
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
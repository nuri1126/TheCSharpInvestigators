using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// Privacy Page Model for the Privacy Razor Page
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Logger property 
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Creates a Privacy logger which uses a log category equal to the name of the model.
        /// </summary>
        /// <param name="logger">An instance of the built-in ILogger service to use</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// TODO ...
        /// </summary>
        public void OnGet()
        {
        }
    }
}

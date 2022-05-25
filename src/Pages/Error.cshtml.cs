using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary>
    /// Error PageModel for the Error Razor Page.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // A property that holds the RequestID.
        public string requestedId { get; set; }

        /// <summary>
        /// Holds True if RequestID is "not null or empty" or false otherwise.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(requestedId);

        // Logger property 
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        ///  Creates an Error logger which uses a log category equal to the name of the model.
        /// </summary>
        /// <param name="logger">An instance of the built-in ILogger service to use</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a RequstID.
        /// </summary>
        public void OnGet()
        {
            requestedId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
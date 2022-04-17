using System.Collections;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;

namespace LetsGoSEA.WebSite.Pages
{
    /// <summary> 
    /// Nuli Bang
    /// <summary>
    /// /// <summary> 
    /// Nirmalya Ghosh
    /// <summary> 
    /// <summary>
    /// Zi Wang
    /// <summary>
    /// <summary>
    /// Evan Marshall
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly NeighborhoodService _neighborhoodService;
        public IEnumerable<Neighborhood> Neighborhoods { get; private set; }

        public IndexModel(ILogger<IndexModel> logger,
            NeighborhoodService neighborhoodService)
        {
            _logger = logger;
            _neighborhoodService = neighborhoodService;
        }

        // public IEnumerable<ProductModel> Products { get; private set; }
        

        public void OnGet()
        {
            Neighborhoods = _neighborhoodService.GetNeighborhoods();
        }
    }
}
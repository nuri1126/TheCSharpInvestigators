using System.Collections.Generic;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    public class Index : PageModel
    {

        private NeighborhoodService NeighborhoodService { get;  }
        
        public Index(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }
        
        public IEnumerable<NeighborhoodModel> Neighborhoods { get; private set; }

        public void OnGet()
        {
            Neighborhoods = NeighborhoodService.GetNeighborhoods();
        }
    }
}
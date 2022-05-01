using Microsoft.AspNetCore.Mvc.RazorPages;
using LetsGoSEA.WebSite.Models;
using LetsGoSEA.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetsGoSEA.WebSite.Pages.Neighborhood
{
    public class Read : PageModel
    {
        private NeighborhoodService NeighborhoodService { get; }

        public Read(NeighborhoodService neighborhoodService)
        {
            NeighborhoodService = neighborhoodService;
        }

        public NeighborhoodModel Neighborhood;

        public void OnGet(int id)
        {
            Neighborhood = NeighborhoodService.GetNeighborhoodById(id);
        }
    }
}
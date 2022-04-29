using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LetsGoSEA.WebSite.Data;
using LetsGoSEA.WebSite.Models;

namespace LetsGoSEA.WebSite.Views.Neighborhoods
{
    public class DetailsModel : PageModel
    {
        private readonly LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext _context;

        public DetailsModel(LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext context)
        {
            _context = context;
        }

        public NeighborhoodModel NeighborhoodModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NeighborhoodModel = await _context.NeighborhoodModel.FirstOrDefaultAsync(m => m.Id == id);

            if (NeighborhoodModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

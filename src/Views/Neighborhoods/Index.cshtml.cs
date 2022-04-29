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
    public class IndexModel : PageModel
    {
        private readonly LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext _context;

        public IndexModel(LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext context)
        {
            _context = context;
        }

        public IList<NeighborhoodModel> NeighborhoodModel { get;set; }

        public async Task OnGetAsync()
        {
            NeighborhoodModel = await _context.NeighborhoodModel.ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LetsGoSEA.WebSite.Data;
using LetsGoSEA.WebSite.Models;

namespace LetsGoSEA.WebSite.Views.Neighborhoods
{
    public class CreateModel : PageModel
    {
        private readonly LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext _context;

        public CreateModel(LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NeighborhoodModel NeighborhoodModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.NeighborhoodModel.Add(NeighborhoodModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LetsGoSEA.WebSite.Data;
using LetsGoSEA.WebSite.Models;

namespace LetsGoSEA.WebSite.Views.Neighborhoods
{
    public class EditModel : PageModel
    {
        private readonly LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext _context;

        public EditModel(LetsGoSEA.WebSite.Data.LetsGoSEAWebSiteContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(NeighborhoodModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NeighborhoodModelExists(NeighborhoodModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool NeighborhoodModelExists(int id)
        {
            return _context.NeighborhoodModel.Any(e => e.Id == id);
        }
    }
}

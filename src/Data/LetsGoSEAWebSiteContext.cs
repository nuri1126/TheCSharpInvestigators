using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LetsGoSEA.WebSite.Models;

namespace LetsGoSEA.WebSite.Data
{
    public class LetsGoSEAWebSiteContext : DbContext
    {
        public LetsGoSEAWebSiteContext (DbContextOptions<LetsGoSEAWebSiteContext> options)
            : base(options)
        {
        }

        public DbSet<LetsGoSEA.WebSite.Models.NeighborhoodModel> NeighborhoodModel { get; set; }
    }
}

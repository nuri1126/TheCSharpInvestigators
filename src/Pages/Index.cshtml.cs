using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
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
        
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }
        public IEnumerable<ProductModel> Products { get; private set; }

        public void OnGet()
        {
            Products = ProductService.GetProducts();
        }
    }
}
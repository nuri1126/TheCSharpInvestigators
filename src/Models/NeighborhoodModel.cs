using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// NeighborhoodModel represents a single neighborhoods and its characteristics
    /// </summary>
    public class NeighborhoodModel
    {
        // Primary key Id
        public int Id { get; set; } = 0;

        // Validating Neighborhood name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a neighborhood name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid neighborhood name.")]
        public string Name { get; set; } = "Default";

        // Image URL 
        //[Required(ErrorMessage = "Please enter an image link. (Put ',' for multiple images)")]
        //[RegularExpression(@"^https://.*$", ErrorMessage = "Please enter a link that starts with https://")]
        public string Image { get; set; } = "Default";

        // Validating City name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a City name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid City name.")]
        public string City { get; set; } = "Default";

        // Validating State name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a State name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid State name.")]
        public string State { get; set; } = "Default";

        // Short description of the neighborhood
        [Required(ErrorMessage = "Please enter a short description.")]
        public string ShortDesc { get; set; } = "Default";

        // Ratings for neighborhood
        public int[] Ratings { get; set; } = null;

        // List of comments  
        public List<CommentModel> Comments { get; set; } = new List<CommentModel> {};

        // Address of neighborhood
        [Required(ErrorMessage = "Please enter a valid address")]
        public string Address { get; set; } = "Default";

        // Image files of neighborhood
        [Display(Name = "Image File")]
        [StringLength(100)]
        public List<string> ImagePath { get; set; } = new List<string> {};
    }
}
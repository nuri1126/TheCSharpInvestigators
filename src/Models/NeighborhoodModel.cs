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
        public int id { get; set; } = 0;

        // Validating Neighborhood name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a neighborhood name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid neighborhood name.")]
        [StringLength(20, ErrorMessage = "A Neighborhood name cannot exceed 20 characters. ")]
        public string name { get; set; } = "Default";

        // Image URL
        [RegularExpression(@"(https?:\/\/.*)", ErrorMessage = "Please enter secured urls")]
        public string image { get; set; } = "";

        // Validating City name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a City name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid City name.")]
        public string city { get; set; } = "Seattle";

        // Validating State name to allow only alpha characters
        [Required(ErrorMessage = "Please enter a State name.")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please enter a valid State name.")]
        public string state { get; set; } = "WA";

        // Short description of the neighborhood
        [Required(ErrorMessage = "Please enter a short description.")]
        [StringLength(200, ErrorMessage = "A Neighborhood description cannot exceed 200 characters. ")]
        public string shortDesc { get; set; } = "Default";

        // Ratings for neighborhood
        public int[] ratings { get; set; } = null;

        // List of comments  
        public List<CommentModel> comments { get; set; } = new List<CommentModel> {};

        // Address of neighborhood
        [Required(ErrorMessage = "Please enter a valid address")]
        public string address { get; set; } = "Default";

        // Image files of neighorhood
        public List<UploadedImageModel> uploadedImages { get; set; } = new List<UploadedImageModel>();
    }
}
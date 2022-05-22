namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// Comments users submit on a Neighborhood detail page
    /// </summary>
    public class CommentModel
    {
        // Unique identifier
        public string CommentId { get; set; }

        // Holds the comment string 
        public string Comment { get; set; }
    }
}
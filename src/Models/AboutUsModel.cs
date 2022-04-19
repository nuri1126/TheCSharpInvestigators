using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// About Us Object (represent each team member)
    /// </summary>
    public class AboutUsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LinkedIn { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }

        public override string ToString() => JsonSerializer.Serialize<AboutUsModel>(this);
    }
}
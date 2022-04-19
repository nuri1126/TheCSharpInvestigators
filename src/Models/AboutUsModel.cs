using System.Text.Json;

namespace LetsGoSEA.WebSite.Models
{
    /// <summary>
    /// About Us Object (represent each team member)
    /// </summary>
    public class AboutUsModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Default";
        public string LinkedIn { get; set; } = "Default";
        public string Image { get; set; } = "Default";
        public string Bio { get; set; } = "Default";

        public override string ToString() => JsonSerializer.Serialize<AboutUsModel>(this);
    }
}
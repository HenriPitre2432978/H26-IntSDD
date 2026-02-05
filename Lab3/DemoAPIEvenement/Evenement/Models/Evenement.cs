using System.Text.Json.Serialization;

namespace DemoEvent.Evenement.Models
{
    public class Evenement
    {
        // La toute première étape, celle de préparer le modèle
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("titre")]
        public string Titre { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("lieu")]
        public string Lieu { get; set; } = string.Empty;

        [JsonPropertyName("artiste")]
        public string Artiste { get; set; } = string.Empty;

        [JsonPropertyName("prix")]
        public string Prix { get; set; } = string.Empty;
    }
}

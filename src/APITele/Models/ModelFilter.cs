using System.Text.Json.Serialization;

namespace APITele.Models;

public class ModelFilter
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Genders? Sex { get; set; }
    public int? MaxYear { get; set; }
    public int? MinYear { get; set; }
}

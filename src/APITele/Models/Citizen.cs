using System.Text.Json.Serialization;

namespace APITele.Models;


public enum Genders
{
    Male,
    Female
}
public class Citizen
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Genders Sex { get; set; }
    public Citizen(string id, string name, int age, Genders sex)
    {
        Id = id;
        Name = name;
        Age = age;
        Sex = sex;
    }
}

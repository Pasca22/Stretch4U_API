using Stretch4U.Data.Models.Users;
using System.Text.Json.Serialization;

namespace Stretch4U.Data.Models.ExercisesImages;

public class HamstringStretchImage
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
}

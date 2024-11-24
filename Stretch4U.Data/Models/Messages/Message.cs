using Stretch4U.Data.Models.Users;
using System.Text.Json.Serialization;

namespace Stretch4U.Data.Models.Messages;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
}

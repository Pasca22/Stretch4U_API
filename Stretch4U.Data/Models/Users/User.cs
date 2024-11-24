using Stretch4U.Data.Models.Messages;
using System.Text.Json.Serialization;

namespace Stretch4U.Data.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } 
    public string Password { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}

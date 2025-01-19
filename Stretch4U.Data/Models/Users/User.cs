using Stretch4U.Data.Models.ExercisesImages;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.Data.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } 
    public string Password { get; set; }
    public int? MobilityLevel { get; set; } 
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<HamstringStretchImage> HamstringStretchImages { get; set; }
}

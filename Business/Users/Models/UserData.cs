using Stretch4U.Data.Models.Exercises;

namespace Stretch4U.Business.Users.Models;

public class UserData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public JwtToken Token { get; set; }
    public int Result { get; set; }
    public List<Exercise> Exercises { get; set; }
}

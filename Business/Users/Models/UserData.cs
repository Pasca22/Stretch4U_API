using Stretch4U.Data.Models.Messages;

namespace Stretch4U.Business.Users.Models;

public class UserData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public JwtToken Token { get; set; }
}

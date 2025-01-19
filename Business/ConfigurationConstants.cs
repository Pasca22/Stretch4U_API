namespace Stretch4U.Business;

public class ConfigurationConstants
{
    public static string JwtIssuer { get; set; } = "Jwt:Issuer";
    public static string JwtAudience { get; set; } = "Jwt:Audience";
    public static string JwtKey { get; set; } = "Jwt:Key";
    public static string ChatBotKey { get; set; } = "ChatBot:Key";
    public static string ChatBotUrl { get; set; } = "ChatBot:Url";
    public static string ChatBotModel { get; set; } = "ChatBot:Model";
    public static string AIModelUrl { get; set; } = "AI_Model:Url";
}

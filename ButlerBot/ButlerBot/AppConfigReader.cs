using System.Configuration;
namespace ButlerBot;

public static class AppConfigReader
{
    public static readonly string AccessToken = ConfigurationManager.AppSettings["token"];
}

using Microsoft.Extensions.Configuration;

namespace PortalCommon
{
    public static class AppSettings
    {
        public static string FilePath { get; private set; }
        public static string DBConnection { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            FilePath = configuration.GetValue<string>("DataSettings:FilePath");

            DBConnection = configuration.GetValue<string>("DataSettings:DBConnection");
        }
    }

}

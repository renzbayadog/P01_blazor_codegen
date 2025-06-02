using codegeneratorlib.Models;
using Serilog;

namespace codegeneratorlib.Helpers
{
    public class AppHelper
    {
        public static IConfiguration _config;
        //public static IHttpContextAccessor _httpContextAccessor;

        public AppHelper(IConfiguration config)
        {
            //_httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        
        public static CDNFTPConfigurationVM CDNFTPConfiguration
        {
            get
            {
                return new CDNFTPConfigurationVM
                {
                    Username = _config["FtpClient:UserName"],
                    Password = _config["FtpClient:Password"],
                    DriveDirectory = _config["FtpClient:DriveDirectory"],
                    PublicHost = _config["FtpClient:SiteHostName"],
                    FileHostName = _config["FtpClient:FileHostName"]
                };
            }
        }

        public static FTPFolderVM CDNFTPFolder
        {
            get
            {
                return new FTPFolderVM
                {
                    DocumentLocation = _config["FtpClient:OutputDirectoryDocuments"]
                };
            }
        }

        public static void LogMessage(string message)
		{
            var filedirectory = _config["AppLogger:AppLogDirectory"].ToString();
            var filename = "ApiLogger.txt";

            var fullpath = Path.Combine(filedirectory, filename);
            
            Directory.CreateDirectory(filedirectory);

            if (!File.Exists(fullpath))
            {
                File.Create(fullpath);
            }

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(fullpath)
            .CreateLogger();

            Log.Information(message);
		}
    }
}

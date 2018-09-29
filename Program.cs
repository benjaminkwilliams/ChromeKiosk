using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeKiosk
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public CLIOptions ParseOptions(string[] args)
        {
            CLIOptions _return = new CLIOptions();

            return _return;
        }

        public void ShowHelp()
        {
            Console.WriteLine("ChromeKiosk" + Environment.NewLine +
                              "\t-h:                                    Show Help" + Environment.NewLine +
                              "\t-p:\"C:\\CKSettings\\CKSettings.json\" Path to CKSettings file" + Environment.NewLine
                              );
        }
    }

    public class CLIOptions
    {
        bool ShowHelp;
        string CKSettingsPath;
    }

    public class CKSettings
    {
        List<string> ChromeOptions;
        List<SiteSettings> SiteSettings;
    }

    public class SiteSettings
    {
        uint TimeoutSeconds;
        uint TimeoutMinutes;

        uint ViewTimeSeconds;
        uint ViewTimeMinutes;

        string Uri;
        string UriSettings;
    }
}

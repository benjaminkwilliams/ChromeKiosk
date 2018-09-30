using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ChromeKiosk
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIOptions opt = ParseOptions(args);
            if (opt.ShowHelp)
            {
                ShowHelp();
                return;
            }

        }

        /// <summary>
        /// Parse the Command LIne Options
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Options objects</returns>
        public static CLIOptions ParseOptions(string[] args)
        {
            CLIOptions _return = new CLIOptions()
            {
                ShowHelp = false,
                CKSettingsPath = Path.Combine(AppContext.BaseDirectory, "CKSettings.json")
            };

            foreach (string s in args)
            {
                if (s.Length < 2)
                {
                    _return.ShowHelp = true;
                    Console.WriteLine("Unknown Argument: \"" + s + "\"");
                    break;
                }
                if (s.Length == 2)
                {
                    // support the common help flags in Windows
                    if (s == "-h" || s == "/h" || s == "-?" || s == "/?" )
                    {
                        _return.ShowHelp = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unknown Argument: \"" + s + "\"");
                        _return.ShowHelp = true;
                        break;
                    }
                }

                if (s.Length >= 3)
                {
                    
                    if (s.Substring(0,3) == "-p:" && s.Length > 4)
                    {
                        _return.CKSettingsPath = s.Substring(3, s.Length - 3);
                    }
                    else
                    {
                        Console.WriteLine("Unknown Argument: \"" + s + "\"");
                        _return.ShowHelp = true;
                        break;
                    }
                }
            }

            return _return;
        }

        /// <summary>
        /// Show the Help
        /// </summary>
        public static void ShowHelp()
        {
            Console.WriteLine("ChromeKiosk" + Environment.NewLine +
                              "\t-h                                 Show Help" + Environment.NewLine +
                              "\t-p:\"C:\\CKSettings\\CKSettings.json\" Path to CKSettings file" + Environment.NewLine
                              );
        }
    }

    /// <summary>
    /// The Object form of the Command Lint Options
    /// </summary>
    public class CLIOptions
    {
        public bool ShowHelp;
        public string CKSettingsPath;
    }

    /// <summary>
    /// The Object from of the JSON file
    /// </summary>
    public class CKSettings
    {
        public List<string> ChromeOptions;
        public List<SiteSettings> SiteSettings;
    }

    /// <summary>
    /// The Object form of the SiteSettings from the JSON
    /// </summary>
    public class SiteSettings
    {
        public uint TimeoutSeconds;
        public uint TimeoutMinutes;

        public uint ViewTimeSeconds;
        public uint ViewTimeMinutes;

        public string Uri;
        public string UriSettings;
    }
}

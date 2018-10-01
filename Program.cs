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
            CKSettings _Settings = null;
            if (opt.ShowHelp)
            {
                ShowHelp();
                return;
            }
            else if (opt.CreateDemo)
            {
                _Settings = CreateDemo(opt.CKSettingsPath);
            }
            else
            {
                _Settings = ParseSettings(opt.CKSettingsPath);
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
                CreateDemo = false,
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
                        _return.CKSettingsPath = s.Substring(3, s.Length - 3).Replace("\"", "").Replace("'", "");
                    }
                    else if (s.Substring(0, 3) == "-d:" && s.Length > 4)
                    {
                        _return.CreateDemo = true;
                        _return.CKSettingsPath = s.Substring(3, s.Length - 3).Replace("\"", "").Replace("'", ""); ;
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
                              "\t-p:\"C:\\CKSettings\\CKSettings.json\" Path to CKSettings file" + Environment.NewLine +
                              "\t-d:\"C:\\CKSettings\\CKSettings.json\" Create a demo file at Path" + Environment.NewLine
                              );
        }

        /// <summary>
        /// Create a demo JSON file
        /// </summary>
        /// <returns></returns>
        public static CKSettings CreateDemo(string PathToFile)
        {
            if (File.Exists(PathToFile))
            {
                throw new Exception("Settings file\t" + PathToFile + " already exists.");
            }

            CKSettings demo = new CKSettings()
            {
                ChromeOptions = new List<string>
                {
                    "--log-level=3", // Turn down logging
                    "--silent", // Turn off logging
                    "--kiosk", // Run as Kiosk
                    "--disable-infobars" // Turn off automation notice
                },
                SiteSettings = new List<SiteSettings>()
            };

            demo.SiteSettings.Add(new SiteSettings {
                Uri = "",
                TimeoutMinutes = 1,
                ViewTimeMinutes = 1
            });
            demo.SiteSettings.Add(new SiteSettings
            {
                Uri = "https://devrant.com/feed",
                TimeoutMinutes = 1,
                ViewTimeMinutes = 1
            });
            demo.SiteSettings.Add(new SiteSettings
            {
                Uri = "https://public.tableau.com/profile/al.melchior6300#!/vizhome/ALSPTiers/Tier4",
                UriSettings = "?:embed=yes&:refresh=yes&:toolbar=no", // This is ignored on Tableau Public but works on internal sites
                TimeoutMinutes = 1,
                ViewTimeMinutes = 1
            });
            demo.SiteSettings.Add(new SiteSettings
            {
                Uri = "https://public.tableau.com/profile/jodugg3205#!/vizhome/VideoGamesSales/SalesDashboard",
                TimeoutMinutes = 1,
                ViewTimeMinutes = 1
            });

            File.WriteAllText(PathToFile, JsonConvert.SerializeObject(demo, Formatting.Indented));

            return demo;
        }

        /// <summary>
        /// Parse the Settings files from the JSON file
        /// </summary>
        /// <param name="FullPathToFile"></param>
        /// <returns></returns>
        public static CKSettings ParseSettings(string FullPathToFile)
        {
            CKSettings _return = null;
            using (StreamReader stream = new StreamReader(FullPathToFile))
            {
                string payload = stream.ReadToEnd();
                _return = JsonConvert.DeserializeObject<CKSettings>(payload);
            }
            return _return;
        }
    }

    /// <summary>
    /// The Object form of the Command Lint Options
    /// </summary>
    public class CLIOptions
    {
        public bool ShowHelp;
        public bool CreateDemo;
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

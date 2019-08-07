using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Cyriller.Desktop
{
    public class Logger
    {
        public void LogException(Exception ex, string source)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileInfo fi = new FileInfo(assembly.Location);
            string name = $"{assembly.GetName().Name}-{DateTime.UtcNow.ToString("yyyy.MM.dd-HH.mm.ss")}.json";
            string path = Path.Join(fi.Directory.FullName, "Log", name);

            fi = new FileInfo(path);

            if (fi.Exists)
            {
                fi.Delete();
            }
            else if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            object model = new
            {
                Assembly = new
                {
                    Name = assembly.GetName(),
                    assembly.ImageRuntimeVersion,
                    assembly.Location,
                    ProductVersion = productVersion
                },
                Environment = new
                {
                    Environment.MachineName,
                    Environment.OSVersion,
                    Environment.ProcessorCount,
                    Environment.UserDomainName,
                    Environment.UserName,
                    Environment.Version
                },
                Exception = new
                {
                    Source = source,
                    ex.Message,
                    ex.StackTrace,
                    String = ex.ToString()
                }
            };

            string json = JsonConvert.SerializeObject(model, Formatting.Indented);

            using (TextWriter writer = new StreamWriter(fi.FullName))
            {
                writer.Write(json);
            }
        }
    }
}

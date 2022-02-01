using Service.Core.Abstractions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration
{
    /// <summary>
    /// Default implementation for configuration API. It represents an process which expose and REST API listening on port 5000
    /// </summary>
    public class ConfigurationAPI : IConfigurationAPI
    {
        Process configuratorApiProcess;

        public ConfigurationAPI(string executablePath)
        {
            configuratorApiProcess = new Process();
            configuratorApiProcess.StartInfo.FileName = executablePath;

        }

        public void Dispose()
        {
            configuratorApiProcess.Dispose();
        }

        public void Launch()
        {
            try
            {
                configuratorApiProcess.Start();
            }
            catch { }
        }

        public void Stop()
        {
            try
            {
                configuratorApiProcess.Kill();
            }
            catch { }
        }
    }
}

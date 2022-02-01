using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration
{
    /// <summary>
    /// Default implementation for configuration API. It represents an process which expose and REST API listening on port 5000
    /// </summary>
    public class ConfigurationAPIProxy : IConfigurationAPIProxy
    {
        Process configuratorApiProcess;

        private const int MAX_WORKERS_COUNT = 254;

        private string executablePath;
        private string pipeName;

        public IPkcs11Server Server { get; }

        public ConfigurationAPIProxy(IPkcs11Server server, string executablePath)
        {
            this.Server = server;
            this.executablePath = executablePath;

            this.configuratorApiProcess = null;
            this.pipeName = Guid.NewGuid().ToString();

            Task.Run(ListeningLoop);
        }

        public void Dispose()
        {
            StopProcess();
        }

        public void Launch()
        {
            try
            {
                CreateProcess();

                configuratorApiProcess.Start();
            }
            catch(Exception e) { 
            }
        }

        public void Stop()
        {
            try
            {
                StopProcess();
            }
            catch(Exception e) { 
            }
        }

        #region Private

        private void ListeningLoop()
        {
            while (true)
            {
                NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, MAX_WORKERS_COUNT);

                pipeServer.WaitForConnection();

                Task.Run(() => Executor(pipeServer));
            }
        }

        private void Executor(NamedPipeServerStream pipeServerStream)
        {
            using StreamWriter writer = new StreamWriter(pipeServerStream);
            using StreamReader reader = new StreamReader(pipeServerStream);

            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("Good");
            writer.Flush();

            pipeServerStream.WaitForPipeDrain();
            pipeServerStream.Close();
        }

        private void CreateProcess()
        {
            StopProcess();

            configuratorApiProcess = new Process();
            configuratorApiProcess.StartInfo.FileName = executablePath;


            configuratorApiProcess.StartInfo.Arguments = pipeName;
            configuratorApiProcess.StartInfo.UseShellExecute = false;
        }

        private void StopProcess()
        {
            if (configuratorApiProcess is not null)
            {
                configuratorApiProcess.Kill();
            }

            configuratorApiProcess = null;
        }

        #endregion
    }
}

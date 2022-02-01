using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Configuration
{
    /// <summary>
    /// Represents the API used to configure the server
    /// </summary>
    public interface IConfigurationAPI : IDisposable
    {
        public void Launch();

        public void Stop();
    }
}

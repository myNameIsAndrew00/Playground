using Service.Core.Abstractions.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Configuration
{
    /// <summary>
    /// Represents the API component used to configure the server. It can be attached to a pkcs11 server and exposed via a different process or directly from the pkcs11 server.
    /// </summary>
    public interface IConfigurationAPIProxy : IDisposable
    {
        /// <summary>
        /// Represents the server which will be exposed via this API.
        /// </summary>
        public IConfigurablePkcs11Server Server { get; }

        public void Launch();

        public void Stop();
    }
}

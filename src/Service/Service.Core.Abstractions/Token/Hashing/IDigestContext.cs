using Service.Core.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token.Hashing
{
    /// <summary>
    /// Provides methods and properties for an hashing context.
    /// </summary>
    public interface IDigestContext : IContext
    {
        public bool LengthRequest { get; set; }
    }
}

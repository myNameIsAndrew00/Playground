using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token.Encryption
{
    public interface IAesMechanismOptions : IMechanismOptions
    {
        public byte[] IV => this.Data.Value;
    }
}

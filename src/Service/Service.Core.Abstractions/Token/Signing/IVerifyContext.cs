using Service.Core.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token.Signing
{
    public interface IVerifyContext : IContext
    {
        public bool LengthRequest { get; set; }

        public byte[] VerifyData { get; set; }
    }
}

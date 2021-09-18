using Service.Core.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token.Encryption
{
    public interface IKeyContext : IContext
    {
        /// <summary>
        /// Specify if the operation should only return the length of the data processed
        /// </summary>
        public bool LengthRequest { get; set; }
    }
}

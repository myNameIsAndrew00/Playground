using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Models
{
    public class SessionDTO
    { 
        public ulong Id { get; set; }

        public bool Closed { get; set; }
    }
}

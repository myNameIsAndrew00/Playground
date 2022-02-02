using Service.Core.Abstractions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration.Models
{
    public class SessionDAO
    {
        public SessionDAO() { }

        public SessionDAO(IReadOnlySession session)
        {
            this.Id = session.Id;
            this.Closed = session.Closed;
        }

        public ulong Id { get; set; }

        public bool Closed { get; set; }

    }
}

using Service.ConfigurationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel.Pages.Session
{
    public class SessionItemViewModel : BaseViewModel
    {
       
        public SessionItemViewModel(SessionDTO sessionDTO)
        {
            this.Id = sessionDTO.Id;
            this.TimeStamp = sessionDTO.TimeStamp.ToString(DATETIME_FORMAT);
            this.IsClosed = sessionDTO.Closed;

        }

        public ulong Id { get; set; }

        public string TimeStamp { get; set; }

        public string Status => IsClosed ? "Closed" : "Opened";

        public bool IsClosed { get; set; }

    }
}

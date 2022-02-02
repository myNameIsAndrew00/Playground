using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel
{
    public class ApplicationViewModel : BaseViewModel
    {

        public bool CurrentPageShouldAnimateOut { get; set; } = false;

        public ApplicationPages CurrentPage { get; set; } = ApplicationPages.Connect;
 
        public async Task ChangePage(ApplicationPages NewPage)
        {
            CurrentPageShouldAnimateOut = true;
            await Task.Delay(800);

            CurrentPage = NewPage;
            CurrentPageShouldAnimateOut = false;
        }

    }
}

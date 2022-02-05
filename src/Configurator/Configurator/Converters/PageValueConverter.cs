using Configurator.ViewModel;
using Configurator.Views.Pages;
using Configurator.Views.Pages.Dashboard;
using Configurator.Views.Pages.Session;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.Converters
{
    public class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BasePage page;

            //return a page inherited from BasePage
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.Connect:
                    page = new Connect();
                    break;
                case ApplicationPages.Dashboard:
                    page = new Dashboard();
                    break;
                case ApplicationPages.Sessions:
                    page = new Sessions();
                    break;
                case ApplicationPages.Reconnect:
                    page = new Reconnect();
                    break;
                default:
                    return null;
            }


            Application.Instance.CurrentPageContext = (page.DataContext as BaseViewModel);

            return page;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

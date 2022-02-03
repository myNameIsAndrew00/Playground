using Configurator.ViewModel;
using Configurator.Views.Pages;
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
            //return a page inherited from BasePage
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.Connect:
                    return new Connect();
                case ApplicationPages.Dashboard:
                    return new Dashboard();
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

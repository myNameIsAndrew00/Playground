using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Configurator.Converters
{
    public class BooleanToColorConverter : BaseValueConverter<BooleanToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter == null ?
                (bool)value ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red :
                (bool)value ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Green; ;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

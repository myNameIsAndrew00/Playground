using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Configurator.Converters
{
    public class LogLevelToColorConverter : BaseValueConverter<LogLevelToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LogLevel logLevel = (LogLevel)value;

            switch (logLevel)
            {
                case LogLevel.Info:
                    return Brushes.DarkGreen;
                case LogLevel.Error:
                    return Brushes.Red;
                case LogLevel.Warning:
                    return Brushes.Beige;
                default:
                    return Brushes.Black;
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.Converters
{
    public class StringToIconConverter : BaseValueConverter<StringToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueString = value as string;

            switch (valueString)
            {
                case "session": return IconChar.Buffer;
                case "logs": return IconChar.ClipboardList;
                case "certificate": return IconChar.Certificate;
                case "disconnect": return IconChar.SignOutAlt;

                default: return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

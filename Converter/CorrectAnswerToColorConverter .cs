using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp
{
    class CorrectAnswerToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isCorrectAnswer = (bool)value;

            return isCorrectAnswer ? Color.FromRgba(0, 128, 0, 255) : Color.FromRgba(255, 0, 0, 255); // Green and Red in ARGB
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

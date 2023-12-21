using MblexApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp
{
    class ChoiceToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string choice)
            {
                // Check if the choice matches the correct answer
              //  var question = (PublicQuestion)parameter;
               // bool isCorrectChoice = question.Choices. == choice;

              //  return isCorrectChoice ? Color.FromRgba(0, 128, 0, 255) : Color.FromRgba(255, 0, 0, 255); // Green and Red in ARGB
            }

            return Color.FromRgba(0, 0, 0, 255);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

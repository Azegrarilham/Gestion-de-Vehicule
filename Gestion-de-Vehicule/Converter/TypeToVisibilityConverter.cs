using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Gestion_de_Vehicule.Converter
{
    internal class TypeToVisibilityConverter: IValueConverter
    {
        // Converts the selected vehicle type to Visibility based on the parameter, to dynamically show or hide input fields based on the selected vehicle type.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            string selectedType = value.ToString();
            string expectedType = parameter.ToString();

            return selectedType == expectedType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

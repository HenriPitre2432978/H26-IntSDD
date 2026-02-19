using StudentClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentClient.ViewModel.Converters
{
    /// <summary>
    /// Converti une chaîne reçue en booléen en la comparant à un paramètre de comparaison. Et vice-versa.
    /// </summary>
    public class FullNameToPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || value is not string fullName || parameter is not string partName)
                return "";

            // (format "First Last")
            var names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (names.Length == 0)
                return "";

            switch (partName)
            {
                case "FName":
                    return names[0];
                case "LName":
                    return names.Length > 1 ? names[names.Length - 1] : "";
                default:
                    return fullName;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
        
    }
}

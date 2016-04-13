using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Turn_Timer_WPF
{
    /// <summary>
    /// System for converting radio-button non-bool values to bools.
    /// </summary>
    public class BoolToIntConverter : IValueConverter
    {
        public BoolToIntConverter() { }

        public object Convert( object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value is bool )
                return !( bool ) value;

            return value;
        }

        public object ConvertBack( object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value is bool )
                return !( bool ) value;

            return value;
        }
    }
}

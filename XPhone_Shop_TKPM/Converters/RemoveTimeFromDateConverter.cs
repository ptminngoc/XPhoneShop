﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace XPhone_Shop_TKPM.Converters
{
    class RemoveTimeFromDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //DateTime date = (DateTime)value;
           // string res = date.ToShortDateString();

            string temp = (string) value;
            string[] res = temp.Split(' ');

          
            return res[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

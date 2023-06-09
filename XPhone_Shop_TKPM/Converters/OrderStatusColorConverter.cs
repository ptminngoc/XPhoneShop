﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace XPhone_Shop_TKPM.Converters
{
    public class OrderStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (string)value;
            switch (status)
            {
                case "Mới tạo":
                    return "#00FD9C";
                case "Ðã thanh toán":
                    return "#02C87C";
                case "Đã hủy":
                    return "#CB3737";
                default:
                    return "#9BFFD9";
            }
        }

        public object ConvertBack(
        object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

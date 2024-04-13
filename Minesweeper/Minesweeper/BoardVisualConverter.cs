using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
    public class BoardVisualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;

            if (intValue >= 0 && intValue <= 8)
            {
                SolidColorBrush fontColor;
                switch (intValue)
                {
                    case 1:
                        fontColor = Brushes.Blue;
                        break;
                    case 2:
                        fontColor = Brushes.Green;
                        break;
                    case 3:
                        fontColor = Brushes.Red;
                        break;
                    case 4:
                        fontColor = Brushes.DarkBlue;
                        break;
                    case 5:
                        fontColor = Brushes.Brown;
                        break;
                    case 6:
                        fontColor = Brushes.Teal;
                        break;
                    case 7:
                        fontColor = Brushes.Black;
                        break;
                    case 8:
                        fontColor = Brushes.Gray;
                        break;
                    default:
                        fontColor = Brushes.White;
                        break;
                }

                return fontColor;
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

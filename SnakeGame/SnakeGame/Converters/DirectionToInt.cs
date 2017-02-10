using System;
using System.Globalization;
using System.Windows.Data;
using SnakeGame.GameObjects.Enums;

namespace SnakeGame.Converters
{
    public class DirectionToInt : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            Directions direction = (Directions)value;
            if (targetType != typeof(double))
            {
                throw new ArgumentException("Invalid data");
            }
            double directionValue = 90;
            switch (direction)
            {
                case Directions.Up:
                    directionValue = 270;
                    break;
                case Directions.Right:
                    directionValue = 0;
                    break;
                case Directions.Down:
                    directionValue = 90;
                    break;
                case Directions.Left:
                    directionValue = 180;
                    break;
                default:
                    break;
            }
            return directionValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

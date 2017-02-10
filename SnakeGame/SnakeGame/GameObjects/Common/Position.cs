using SnakeGame.ViewModels;
using SnakeGame.Constants;

namespace SnakeGame.GameObjects.Common
{
    public class Position : BaseViewModel
    {
        private double x;
        private double y;
        public Position(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X
        {
            get
            {
                return this.x;


            }
            set
            {
                var newValue = value;
                if (newValue < 0)
                {
                    newValue += BaseConstants.MaxX;
                }
                else if (newValue >= BaseConstants.MaxX)
                {
                    newValue %= BaseConstants.MaxX;
                }
                this.x = newValue;
                this.OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                var newValue = value;
                if (newValue < 0)
                {
                    newValue += BaseConstants.MaxY;
                }
                else if (newValue >= BaseConstants.MaxY)
                {
                    newValue %= BaseConstants.MaxY;
                }

                this.y = newValue;
                this.OnPropertyChanged("Y");
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position))
            {
                return false;
            }

            Position other = (Position)obj;

            return this.X == other.x && this.Y == other.Y;
        }

        public override string ToString()
        {
            return $"({this.x}, {this.y})";
        }
    }
}

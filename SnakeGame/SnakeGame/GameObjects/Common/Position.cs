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
                //if (value < 0)
                //{
                //    throw new ArgumentException("X cannot be less than 0", "x");
                //}
                //else if (value > BaseConstants.MaxX)
                //{
                //    throw new ArgumentException("X cannot be greater than Max X", "x");
                //}

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
                //if (value < 0)
                //{
                //    throw new ArgumentException("Y cannot be less than 0", "y");
                //}
                //else if (value > BaseConstants.MaxY)
                //{
                //    throw new ArgumentException("Y cannot be greater than Max Y", "y");
                //}
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
            return string.Format("({0}, {1})", this.x, this.y);
        }
    }
}

using System;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Abstract;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Contracts;
using SnakeGame.GameObjects.Enums;

namespace SnakeGame.GameObjects.Snake
{
    public class SnakeSegment : GameObject, IMovable
    {
        private int speed;

        public SnakeSegment(Position pos, int size, int speed, Directions direction)
            : base(pos, size)
        {
            this.Speed = speed;
            this.Direction = direction;
        }

        public SnakeSegment(Position pos, int size, int speed)
            : this(pos, size, speed, Directions.Right)
        {
        }

        public int Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Speed cannot be less or equal to 0", "Speed");
                }
                this.speed = value;
                this.OnPropertyChanged("Speed");

            }
        }

        public Directions Direction { get; set; }

        public void Move()
        {
            var deltaX = BaseConstants.DX[this.Direction];
            var deltaY = BaseConstants.DY[this.Direction];

            this.Position.X += deltaX * this.speed;
            this.Position.Y += deltaY * this.speed;

            this.OnPropertyChanged("X");
            this.OnPropertyChanged("Y");
        }

        public void ChangeDirection(Directions direction)
        {
            var dirIndex = (int)direction;
            var currentDirIndex = (int)this.Direction;

            var dirDifference = Math.Abs(dirIndex - currentDirIndex);
            if (dirDifference != 2)
            {
                this.Direction = direction;
            }

            this.OnPropertyChanged("Direction");
        }

        public override string ToString()
        {
            return this.Position.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Contracts;
using SnakeGame.GameObjects.Enums;
using SnakeGame.GameObjects.Obstacles.StaticObstacles;

namespace SnakeGame.GameObjects.Obstacles.MovingObstacles
{
    public class MovingObstacle : Obstacle, IMovable
    {
        private int speed;
        public MovingObstacle(Position pos, int size, int speed)
            : this(pos, size, speed, Directions.Right)
        {
        }

        public MovingObstacle(Position pos, int size, int speed, Directions direction)
            : base(pos, size)
        {
            this.Speed = speed;
            this.Direction = direction;
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
        }

        public void ChangeDirection(Directions direction)
        {
            var dirIndex = (int)direction;
            var currentDirIndex = (int)this.Direction;

            var dirDifference = Math.Abs(dirIndex - currentDirIndex);
            //if (dirDifference != 2)
            //{
            this.Direction = direction;
            //}
        }
    }

}

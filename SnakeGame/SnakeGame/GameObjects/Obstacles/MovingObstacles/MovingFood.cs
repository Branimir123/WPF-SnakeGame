using System;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Contracts;
using SnakeGame.GameObjects.Enums;
using SnakeGame.GameObjects.Obstacles.StaticObstacles;

namespace SnakeGame.GameObjects.Obstacles.MovingObstacles
{
    public class MovingFood : Food, IMovable
    {
        private int speed;

        public MovingFood(Position pos, int size, int speed)
            : this(pos, size, speed, Directions.Right)
        {

        }

        public MovingFood(Position pos, int size, int speed, Directions direction)
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
                //this.OnPropertyChanged("Speed");
            }
        }

        public Directions Direction
        {
            get;
            set;
        }

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
            this.Direction = direction;
        }
    }

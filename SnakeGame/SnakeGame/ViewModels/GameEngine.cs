using System;
using System.Windows.Threading;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Enums;
using SnakeGame.GameObjects.Snake;

namespace SnakeGame.ViewModels
{
    public class GameEngine : BaseViewModel
    {
        private static readonly Random random = new Random();

        private DispatcherTimer timer;

        public GameEngine()
        {
        }

        public int Heigth => BaseConstants.MaxY;

        public int Width => BaseConstants.MaxX;

        public Snake Snake { get; set; }

        public EnemySnake EnemySnake { get; set; }

        public void ChangeDirection(Directions direction)
        {
            Snake.ChangeDirection(direction);
        }

        //Main game logic goes below: 
        public void StartGame()
        {
            //Initializes the player snake
            InitializeSnake();

            //Initialie the enemy snake 
            InitializeEnemySnake();

            //Check of timer is null
            timer?.Stop();

            //Create the timer and add the tick event delegate
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            int ticksCounter = 0;

            //Do this on every tick of the timer;
            timer.Tick += delegate
            {
                //At every tick move the snake and the enemy snake
                Snake.Move();
                EnemySnake.Move();
               
                //Increase counter ticks
                ticksCounter++;
                

              
                //Check if snake is dead
                //var isDead = CheckSnakeDead();
                //if (isDead)
              //  {
              //      GameOver();
              //  }

                this.OnPropertyChanged("Snake");
                this.OnPropertyChanged("EnemySnake");
            };

            //Start the timer
            timer.Start();
        }

        //Initializes the Player snake
        private void InitializeSnake()
        {
            var x = BaseConstants.MaxX / 2;
            var y = BaseConstants.MaxY / 2;
            var position = new Position(x, y);
            var size = BaseConstants.DefaultSnakeSize;
            var speed = BaseConstants.DefaultSnakeSpeed;
            this.Snake = new Snake(position, size, speed);
        }

        private void InitializeEnemySnake()
        {
            var x = BaseConstants.MaxX / 4;
            var y = BaseConstants.MaxY / 4;
            var position = new Position(x, y);
            var size = BaseConstants.DefaultSnakeSize;
            var speed = BaseConstants.DefaultSnakeSpeed;
            this.EnemySnake = new EnemySnake(position, size, speed);
        }
    }
}

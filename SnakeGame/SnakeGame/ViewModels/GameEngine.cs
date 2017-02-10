using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Enums;
using SnakeGame.GameObjects.Obstacles.StaticObstacles;
using SnakeGame.GameObjects.Snake;

namespace SnakeGame.ViewModels
{
    public class GameEngine : BaseViewModel
    {
        private static readonly Random random = new Random();

        private DispatcherTimer timer;

        private ObservableCollection<Obstacle> obstacles;

        private int randomMovingObjectsChecker;

        public GameEngine()
        {
        }

        public int Heigth => BaseConstants.MaxY;

        public int Width => BaseConstants.MaxX;

        public Snake Snake { get; set; }

        public EnemySnake EnemySnake { get; set; }

        public IEnumerable<Obstacle> Obstacles
        {
            get
            {
                return this.obstacles;
            }
            set
            {
                if (this.obstacles == null)
                {
                    this.obstacles = new ObservableCollection<Obstacle>();
                }
                else
                {
                    this.obstacles.Clear();
                }
                foreach (var item in value)
                {
                    this.obstacles.Add(item);
                }
                this.OnPropertyChanged("Obstacles");
            }
        }


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

            //Initializes the obstacles
            InitializeObstacles();

            //Check of timer is null
            timer?.Stop();

            randomMovingObjectsChecker = random.Next(BaseConstants.MinMovingObstaclesPrescaller, BaseConstants.MaxMovingObstaclesPrescaller);



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

                //Change the direction of the enemy snake randomly
                if (ticksCounter == randomMovingObjectsChecker)
                {
                    EnemySnakeChangeDirection();
                    ticksCounter = 0;
                }

                //Increase counter ticks
                ticksCounter++;
                
                //Check if snake is dead
                var isDead = IsSnakeDead();
                if (isDead)
                {
                    GameOver();
                }

                //Notify for property changed
                this.OnPropertyChanged("Snake");
                this.OnPropertyChanged("EnemySnake");
                this.OnPropertyChanged("Obstacles");
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

        private void InitializeObstacles()
        {
            var obstaclesCount = random.Next(BaseConstants.MinObstaclesCount, BaseConstants.MaxObstaclesCount);
            List<Obstacle> obstacles = new List<Obstacle>();
            for (int i = 0; i < obstaclesCount; i++)
            {
                var size = BaseConstants.SquareSize;

                var x = random.Next(BaseConstants.MaxX - size);
                var y = 0;
                do
                {
                    y = random.Next(BaseConstants.MaxY - size - 50);
                }
                while (y >= BaseConstants.MaxY / 2 - BaseConstants.SquareSize && y <= BaseConstants.MaxY / 2 + BaseConstants.SquareSize);

                Position position = new Position(x, y);
                var newObstacle = new Obstacle(position, size);
                obstacles.Add(newObstacle);
            }
            this.Obstacles = obstacles;
        }

        private void EnemySnakeChangeDirection()
        {
            Directions newDirection = (Directions)random.Next(0, 4);
            this.EnemySnake.ChangeDirection(newDirection);
        }

        //Checks if snake is dead
        private bool IsSnakeDead()
        {
            var eaten = HasSnakeEatenItself();
            var crashedInMovingSnakeObstacke = HasSnakeTouchedEnemySnake();
            var outsideOfGameField = HasSnakeLeftTheGameField();
            var crashedInObstacle = HasCrashedInObstacle();

            return eaten || crashedInMovingSnakeObstacke || outsideOfGameField || crashedInObstacle;
        }

        //Checks if snake has eaten itself
        private bool HasSnakeEatenItself()
        {
            var head = this.Snake.Parts[0];
            for (int i = 1; i < this.Snake.Parts.Count; i++)
            {
                var part = this.Snake.Parts[i];
                var isOver = IsOver(head.Position, head.Size,
                    part.Position, part.Size);
                if (isOver)
                {
                    return true;
                }
            }
            return false;
        }

        //Checks if snake has touched enemy snake
        private bool HasSnakeTouchedEnemySnake()
        {
            foreach (var snakePart in this.Snake.Parts)
            {
                foreach (var snakeObstaclePart in this.EnemySnake.Parts)
                {
                    var isOver = IsOver(snakePart.Position, snakePart.Size,
                        snakeObstaclePart.Position, snakeObstaclePart.Size);
                    if (isOver)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasCrashedInObstacle()
        {
            var head = this.Snake.Parts[0];
            foreach (var obstacle in this.Obstacles)
            {
                var isOver = IsOver(head.Position, head.Size,
                    obstacle.Position, obstacle.Size);
                if (isOver)
                {
                    return true;
                }
            }
            return false;
        }
        private bool HasSnakeLeftTheGameField()
        {
            //TODO: Implement logic here
            return false;
        }

        //Checks if element is on top of another element
        private bool IsOver(Position p1, int size1, Position p2, int size2)
        {
            var fX1 = p1.X;
            var fX2 = p1.X + size1 - 1;

            var fY1 = p1.Y;
            var fY2 = p1.Y + size1 - 1;

            var oX1 = p2.X;
            var oX2 = p2.X + size2 - 1;

            var oY1 = p2.Y;
            var oY2 = p2.Y + size2 - 1;

            bool fX1InObstacle = oX1 <= fX1 && fX1 <= oX2;
            bool fX2InObstacle = oX1 <= fX2 && fX2 <= oX2;

            bool fY1InObstacle = oY1 <= fY1 && fY1 <= oY2;
            bool fY2InObstacle = oY1 <= fY2 && fY2 <= oY2;

            return (fX1InObstacle || fX2InObstacle) &&
                   (fY1InObstacle || fY2InObstacle);
        }

        //Stops the game timer
        private void GameOver()
        {
            timer.Stop();
        }
    }
}

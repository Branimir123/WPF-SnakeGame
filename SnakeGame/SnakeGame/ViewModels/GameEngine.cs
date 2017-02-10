using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Enums;
using SnakeGame.GameObjects.Obstacles.MovingObstacles;
using SnakeGame.GameObjects.Obstacles.StaticObstacles;
using SnakeGame.GameObjects.Snake;

namespace SnakeGame.ViewModels
{
    public class GameEngine : BaseViewModel
    {
        private static readonly Random random = new Random();

        private DispatcherTimer timer;

        private ObservableCollection<Obstacle> obstacles;

        private ObservableCollection<MovingObstacle> movingObstacles;

        private ObservableCollection<Food> foods;

        private ObservableCollection<MovingFood> movingFoods;

        private int randomMovingObjectsChecker;

        public GameEngine()
        {
        }

        public int Heigth => BaseConstants.MaxY;

        public int Width => BaseConstants.MaxX;

        //Snakes:
        public Snake Snake { get; set; }

        public EnemySnake EnemySnake { get; set; }

        //Obstacles:
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

        public IEnumerable<MovingObstacle> MovingObstacles
        {
            get
            {
                return this.movingObstacles;
            }
            set
            {
                if (this.movingObstacles == null)
                {
                    this.movingObstacles = new ObservableCollection<MovingObstacle>();
                }
                else
                {
                    this.movingObstacles.Clear();
                }
                foreach (var item in value)
                {
                    this.movingObstacles.Add(item);
                }
                this.OnPropertyChanged("MovingObstacles");
            }
        }

        public IEnumerable<Food> Foods
        {
            get
            {
                return this.foods;
            }
            set
            {
                if (this.foods == null)
                {
                    this.foods = new ObservableCollection<Food>();
                }
                else
                {
                    this.foods.Clear();
                }
                foreach (var item in value)
                {
                    this.foods.Add(item);
                }
                this.OnPropertyChanged("Foods");
            }
        }

        public IEnumerable<MovingFood> MovingFoods
        {
            get
            {
                return this.movingFoods;
            }
            set
            {
                if (this.movingFoods == null)
                {
                    this.movingFoods = new ObservableCollection<MovingFood>();
                }
                else
                {
                    this.movingFoods.Clear();
                }
                foreach (var item in value)
                {
                    this.movingFoods.Add(item);
                }
                this.OnPropertyChanged("MovingFoods");
            }
        }
        private void MoveObstacles()
        {
            foreach (var obstacle in this.MovingObstacles)
            {
                obstacle.Move();
            }

            OnPropertyChanged("MovingObstacles");
        }

        private void MoveFoods()
        {
            foreach (var movingFood in this.MovingFoods)
            {
                movingFood.Move();
            }

            OnPropertyChanged("MovingFoods");
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

            //Initializes the moving obstacles
            InitilizeMovingObstacles();

            ////Initializes the food
            InitilizeFood();

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
                    MovingObstaclesChangeDirection();
                    EnemySnakeChangeDirection();
                    ticksCounter = 0;
                }

                ////Handle moving food movement
                foreach (var movingFood in this.MovingFoods)
                {
                    var shouldChangeDirection = random.Next() % 3 == 0;
                    if (shouldChangeDirection)
                    {
                        Directions newDirection = (Directions)random.Next(0, 4);
                        movingFood.ChangeDirection(newDirection);
                    }
                }

                //Increase counter ticks
                ticksCounter++;

                //Move obstacles which are able to move
                this.MoveObstacles();
                this.MoveFoods();

                ////Check for eaten food or obstacle
                CheckFoodEaten();
                CheckFoodEatenSnakeObstakle();

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

        private void CheckFoodEaten()
        {
            foreach (var food in this.Foods)
            {
                var isOver = IsOver(Snake.Position, Snake.Parts[0].Size,
                    food.Position, food.Size);
                if (isOver)
                {
                    this.Snake.Grow();
                    food.Destroy();
                    ChangeFoodPosition(food);
                    break;
                }
            }

            CheckMovingFoodEaten();
        }
        private void CheckMovingFoodEaten()
        {
            foreach (var movingFood in this.MovingFoods)
            {
                var isOver = IsOver(Snake.Position, Snake.Parts[0].Size,
                    movingFood.Position, movingFood.Size);
                if (isOver)
                {
                    this.Snake.Grow();
                    movingFood.Destroy();
                    ChangeFoodPosition(movingFood);
                    break;
                }
            }
        }
        private void CheckFoodEatenSnakeObstakle()
        {
            foreach (var food in this.Foods)
            {
                var isOver = IsOver(this.EnemySnake.Position, this.EnemySnake.Parts[0].Size,
                    food.Position, food.Size);
                if (isOver)
                {
                    this.EnemySnake.Grow();
                    this.foods.Remove(food);
                    GenerateNewFood();
                    break;
                }
            }
        }

        private void GenerateNewFood()
        {

            Position pos;
            int size = BaseConstants.SquareSize;
            do
            {
                int x = random.Next(BaseConstants.MaxX - size);
                int y = random.Next(BaseConstants.MaxY - size - 50);
                pos = new Position(x, y);
            }
            while (IsFoodOverObstacle(pos, size) || IsFoodOverFood(pos, size));
            Food food = new Food(pos, size);
            food.Position.X = pos.X;
            food.Position.Y = pos.Y;
            this.foods.Add(food);
        }
        private void ChangeFoodPosition(Food food)
        {
            Position pos;
            int size = BaseConstants.SquareSize;
            do
            {
                int x = random.Next(BaseConstants.MaxX - size);
                int y = random.Next(BaseConstants.MaxY - size);
                pos = new Position(x, y);
            }
            while (IsFoodOverObstacle(pos, size));
            food.Position.X = pos.X;
            food.Position.Y = pos.Y;
        }

        //Functions to initialize the game objects
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

        private void InitilizeMovingObstacles()
        {
            var movingObstaclesCount =
                random.Next(BaseConstants.MinMovingObstaclesCount,
                BaseConstants.MaxMovingObstaclesCount);
            List<MovingObstacle> movingObstacles = new List<MovingObstacle>();
            for (int i = 0; i < movingObstaclesCount; i++)
            {
                var size = BaseConstants.SquareSize;

                var x = random.Next(BaseConstants.MaxX - size);
                var y = random.Next(BaseConstants.MaxY - size);
                Position position = new Position(x, y);
                var newObstacle = new MovingObstacle(position, size, random.Next(BaseConstants.MinMovingObstaclesSpeed, BaseConstants.MaxMovingObstaclesSpeed));
                movingObstacles.Add(newObstacle);
            }
            this.MovingObstacles = movingObstacles;
        }

        private void InitilizeFood()
        {
            var foods = new List<Food>();

            var foodCount = BaseConstants.FoodCount;
            for (int i = 0; i < foodCount; i++)
            {
                int size = BaseConstants.SquareSize;
                int x = random.Next(BaseConstants.MaxX - size);
                int y = random.Next(BaseConstants.MaxY - size - 50);
                Position pos = new Position(x, y);
                bool isOver;
                if (foods.Count == 0)
                {
                    isOver = IsFoodOverObstacle(pos, size);
                }
                else
                {
                    isOver = IsFoodOverObstacle(pos, size) || IsFoodOverFood(pos, size);
                }
                if (!isOver)
                {
                    Food food = new Food(pos, size);
                    foods.Add(food);
                    this.Foods = foods;
                }
                else
                {
                    i--;
                }
            }

            this.Foods = foods;

            //Added InitializeMovingFood();
            InitializeMovingFood();
        }

        private void InitializeMovingFood()
        {
            var movingFoods = new List<MovingFood>();
            var movingFoodCount = BaseConstants.MovingFoodCount;
            for (int i = 0; i < movingFoodCount; i++)
            {
                int size = BaseConstants.SquareSize;
                int x = random.Next(BaseConstants.MaxX - size);
                int y = random.Next(BaseConstants.MaxY - size);
                Position pos = new Position(x, y);
                var isOver = IsFoodOverObstacle(pos, size);
                if (!isOver)
                {
                    MovingFood movingFood = new MovingFood(pos, size, BaseConstants.MovingFoodsSpeed);
                    movingFoods.Add(movingFood);
                }
                else
                {
                    i--;
                }
            }
            this.MovingFoods = movingFoods;
        }

        private void EnemySnakeChangeDirection()
        {
            Directions newDirection = (Directions)random.Next(0, 4);
            this.EnemySnake.ChangeDirection(newDirection);
        }

        private void MovingObstaclesChangeDirection()
        {
            foreach (var movingObstacle in this.MovingObstacles)
            {
                Directions newDirection = (Directions)random.Next(0, 4);
                movingObstacle.ChangeDirection(newDirection);
            }
        }

        //Checks if snake is dead
        private bool IsSnakeDead()
        {
            var eatenItself = HasSnakeEatenItself();
            var crashedInMovingSnakeObstacle = HasSnakeTouchedEnemySnake();
            var outsideOfGameField = HasSnakeLeftTheGameField();
            var crashedInObstacle = HasCrashedInObstacle();
            var crashedInMovingObstacle = HasSnakeCrashedInMovingObstacle();

            return eatenItself ||
                   crashedInMovingSnakeObstacle ||
                   outsideOfGameField;
        }

        //Helper functions to check if snake is dead:
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
        private bool HasSnakeCrashedInMovingObstacle()
        {
            foreach (var snakePart in this.Snake.Parts)
            {
                foreach (var movingObstacle in this.MovingObstacles)
                {
                    var isOver = IsOver(snakePart.Position, snakePart.Size,
                        movingObstacle.Position, movingObstacle.Size);
                    if (isOver)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsFoodOverObstacle(Position pos, int size)
        {
            foreach (var item in this.Obstacles)
            {
                bool isOver = IsOver(item.Position, item.Size, pos, size);
                if (isOver)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsFoodOverFood(Position pos, int size)
        {
            foreach (var item in this.Foods)
            {
                bool isOver = IsOver(item.Position, item.Size, pos, size);
                if (isOver)
                {
                    return true;
                }
            }
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

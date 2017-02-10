using System.Collections.Generic;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Enums;

namespace SnakeGame.Constants
{
    public static class BaseConstants
    {
        public const int MaxX = 1000;
        public const int MaxY = 540;

        public const int DefaultSnakeSize = 5;
        public const int SquareSize = 15;
        public const int DefaultSnakeSpeed = 15;
        public const int FoodCount = 5;

        public const int MovingFoodCount = 3;

        public static readonly int MinObstaclesCount;
        public static readonly int MaxObstaclesCount;
        public static readonly int MinMovingObstaclesCount;
        public static readonly int MaxMovingObstaclesCount;
        public static readonly int MinMovingObstaclesSpeed;
        public static readonly int MaxMovingObstaclesSpeed;
        public static readonly int MinMovingObstaclesPrescaller;
        public static readonly int MaxMovingObstaclesPrescaller;


        public static readonly int MovingFoodsSpeed;


        private static readonly Position SnakeStartPosition;

        public static readonly Dictionary<Directions, int> DX;
        public static readonly Dictionary<Directions, int> DY;

        static BaseConstants()
        {
            DX = new Dictionary<Directions, int>
            {
                [Directions.Right] = 1,
                [Directions.Left] = -1,
                [Directions.Down] = 0,
                [Directions.Up] = 0
            };

            DY = new Dictionary<Directions, int>
            {
                [Directions.Right] = 0,
                [Directions.Left] = 0,
                [Directions.Down] = 1,
                [Directions.Up] = -1
            };

            MinObstaclesCount = 5;  //1 * ((MaxY * MaxX) / (SquareSize * SquareSize)) / 100;
            MaxObstaclesCount = 10; //2 * ((MaxY * MaxX) / (SquareSize * SquareSize)) / 100;

            MinMovingObstaclesCount = 5;
            MaxMovingObstaclesCount = 10;

            MinMovingObstaclesSpeed = 1;
            MaxMovingObstaclesSpeed = DefaultSnakeSpeed / 2;

            MovingFoodsSpeed = 7;

            MinMovingObstaclesPrescaller = 20;
            MaxMovingObstaclesPrescaller = 40;
        }
    }
}

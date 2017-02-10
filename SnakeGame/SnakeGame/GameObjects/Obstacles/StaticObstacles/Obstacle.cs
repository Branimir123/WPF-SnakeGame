using SnakeGame.GameObjects.Abstract;
using SnakeGame.GameObjects.Common;

namespace SnakeGame.GameObjects.Obstacles.StaticObstacles
{
    public class Obstacle : GameObject
    {
        public Obstacle(Position pos, int size)
            : base(pos, size)
        {
        }
    }
}

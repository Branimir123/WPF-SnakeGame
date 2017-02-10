using SnakeGame.GameObjects.Abstract;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Contracts;

namespace SnakeGame.GameObjects.Obstacles.StaticObstacles
{
    public class Food : GameObject, IDestroyable
    {
        public Food(Position pos, int size)
            : base(pos, size)
        {
        }

        public void Destroy()
        {
            this.IsDestroyed = true;
        }

        public bool IsDestroyed
        {
            get;
            set;
        }
    }
}

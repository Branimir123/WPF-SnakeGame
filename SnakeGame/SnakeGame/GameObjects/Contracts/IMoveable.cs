using SnakeGame.GameObjects.Enums;

namespace SnakeGame.GameObjects.Contracts
{
    public interface IMovable
    {
        int Speed { get; set; }

        Directions Direction { get; set; }

        void Move();

        void ChangeDirection(Directions direction);
    }
}

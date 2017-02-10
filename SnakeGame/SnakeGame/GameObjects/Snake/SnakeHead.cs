using SnakeGame.GameObjects.Common;

namespace SnakeGame.GameObjects.Snake
{
    public class SnakeHead : SnakeSegment
    {
        public SnakeHead(Position pos, int size, int speed)
            : base(pos, size, speed)
        {
        }
    }
}

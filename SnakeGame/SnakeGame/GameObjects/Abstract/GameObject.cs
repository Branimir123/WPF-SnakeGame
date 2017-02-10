using System;
using SnakeGame.GameObjects.Common;
using SnakeGame.ViewModels;

namespace SnakeGame.GameObjects.Abstract
{
    public abstract class GameObject : BaseViewModel
    {
        private int size;
        public GameObject(int x, int y, int size)
            : this(new Position(x, y), size)
        {
        }

        public GameObject(Position position, int size)
        {
            this.Position = new Position(position.X, position.Y);
            this.Size = size;
        }

        public virtual int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Size cannot be less or equal to 0", "Size");

                }
                this.size = value;
            }
        }

        public virtual Position Position { get; set; }
    }
}

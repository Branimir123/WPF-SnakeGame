using System.Collections.ObjectModel;
using SnakeGame.Constants;
using SnakeGame.GameObjects.Abstract;
using SnakeGame.GameObjects.Common;
using SnakeGame.GameObjects.Contracts;
using SnakeGame.GameObjects.Enums;

namespace SnakeGame.GameObjects.Snake
{
    public class Snake : GameObject, IMovable, IDestroyable
    {
        private Directions direction;

        private ObservableCollection<SnakeSegment> head;
        public Snake(Position pos, int size, int speed)
            : base(pos, size)
        {
            this.Speed = speed;
            this.IsDestroyed = false;

            this.Parts = new ObservableCollection<SnakeSegment>();
            var head = new SnakeHead(pos, BaseConstants.SquareSize, speed);
            this.Parts.Add(head);
            this.head = new ObservableCollection<SnakeSegment>() { head };

            for (int i = 1; i < size; i++)
            {
                var position = new Position(pos.X - i * BaseConstants.SquareSize, pos.Y);
                var part = new SnakeSegment(position, BaseConstants.SquareSize, this.Speed);
                this.Parts.Add(part);
            }
        }

        public int Speed { get; set; }

        public ObservableCollection<SnakeSegment> Head
        {
            get
            {
                return this.head;
            }
            set
            {
                if (this.head == null)
                {
                    this.head = new ObservableCollection<SnakeSegment>();
                }
                else
                {
                    this.head.Clear();
                }
                foreach (var item in value)
                {
                    this.head.Add(item);
                }
            }
        }

        public int Size
        {
            get
            {
                return Parts.Count;
            }
        }

        public int Score
        {
            get
            {
                return Parts.Count - BaseConstants.DefaultSnakeSize;
            }
        }


        public Directions Direction
        {
            get
            {
                return this.Parts[0].Direction;
            }
            set
            {
                Parts[0].ChangeDirection(value);
            }
        }

        public ObservableCollection<SnakeSegment> Parts { get; set; }

        public override Position Position
        {
            get
            {
                return this.Parts[0].Position;
            }
        }

        public void Grow()
        {
            var tail = this.Parts[this.Parts.Count - 1];
            var x = tail.Position.X;
            var y = tail.Position.Y;
            var direction = tail.Direction;
            var size = tail.Size;

            var newX = x + BaseConstants.SquareSize * (-BaseConstants.DX[direction]);
            var newY = y + BaseConstants.SquareSize * (-BaseConstants.DY[direction]);

            var newTail = new SnakeSegment(new Position(newX, newY), size, Speed, tail.Direction);

            this.Parts.Add(newTail);
            this.OnPropertyChanged("Size");
        }

        public void Destroy()
        {
            this.IsDestroyed = true;
        }

        public void ChangeDirection(Directions direction)
        {
            this.Direction = direction;
        }

        public bool IsDestroyed { get; set; }

        public void Move()
        {
            foreach (var part in this.Parts)
            {
                part.Move();
            }

            for (int i = this.Size - 1; i > 0; i--)
            {
                var part = this.Parts[i];
                var prevPart = this.Parts[i - 1];
                var prevPartDirection = prevPart.Direction;
                part.ChangeDirection(prevPartDirection);
            }

        }
    }
}

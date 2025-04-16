using SnakeGameGPT.Models;
using SnakeGameGPT.Models.Enuns;

namespace SnakeGameGPT.Entities
{
    public class Snake
    {
        private readonly LinkedList<Position> _segments = new();
        private Direction _direction;
        private readonly int _gridWidth;
        private readonly int _gridHeight;


        public IEnumerable<Position> Segments => _segments;
        public Position Head => _segments.First!.Value;

        public Snake(Position start, int gridWidth, int gridHeight)
        {
            _segments.AddFirst(start);
            _direction = Direction.Right;

            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
        }


        public void ChangeDirection(Direction dir)
        {
            if ((_direction == Direction.Up && dir == Direction.Down) ||
                (_direction == Direction.Down && dir == Direction.Up) ||
                (_direction == Direction.Left && dir == Direction.Right) ||
                (_direction == Direction.Right && dir == Direction.Left))
                return;

            _direction = dir;
        }

        public void Move()
        {
            var head = Head;
            int newX = head.X;
            int newY = head.Y;

            switch (_direction)
            {
                case Direction.Up: newY--; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
                case Direction.Right: newX++; break;
            }

            // Faz o wrap nas bordas:
            if (newX < 0) newX = _gridWidth - 1;
            if (newX >= _gridWidth) newX = 0;
            if (newY < 0) newY = _gridHeight - 1;
            if (newY >= _gridHeight) newY = 0;

            var newHead = new Position(newX, newY);


            _segments.AddFirst(newHead);
            _segments.RemoveLast();
        }

        public void Grow()
        {
            for (int i = 0; i < 500; i++)
            {
                _segments.AddLast(_segments.Last!.Value);
            }
        }

        public void TrimOutside(int gridWidth, int gridHeight)
        {
            var valid = _segments.Where(s => s.X < gridWidth && s.Y < gridHeight).ToList();

            _segments.Clear();
            foreach (var s in valid)
                _segments.AddLast(s);
        }

    }
}
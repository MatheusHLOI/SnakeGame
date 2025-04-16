using System;
using System.Windows.Threading;
using System.Windows.Media;
using SnakeGameGPT.Entities;
using SnakeGameGPT.Interfaces;
using SnakeGameGPT.Models;
using SnakeGameGPT.Models.Enuns;

namespace SnakeGameGPT.Engine
{
    public class SnakeGameEngine
    {
        private readonly IGameView _view;
        private readonly DispatcherTimer _timer;
        private readonly Snake _snake;
        private Position _foodPosition;
        private int _score;
        private int _rows;
        private int _cols;
        private bool _startedByPlayer = false;


        public SnakeGameEngine(IGameView view, int rows, int cols, TimeSpan interval)
        {
            _view = view;
            _rows = rows;
            _cols = cols;
            _snake = new Snake(new Position(cols / 2, rows / 2), cols, rows);
            PlaceFood();

            _timer = new DispatcherTimer { Interval = interval };
            _timer.Tick += (s, e) => Update();
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();
        public void ChangeDirection(Direction dir)
        {
            _snake.ChangeDirection(dir);

            if (!_startedByPlayer)
            {
                _startedByPlayer = true;
                _view.OnGameStarted();
                _timer.Start();
            }
        }



        private void Update()
        {
            _snake.Move();
            if (_snake.Head.Equals(_foodPosition))
            {
                _snake.Grow();
                _score++;
                _view.OnScoreChanged(_score); // <- avisa a view
                PlaceFood();
            }

            _view.Clear();
            foreach (var segment in _snake.Segments)
                _view.DrawCell(segment, Brushes.Green);

            _view.DrawCell(_foodPosition, Brushes.Red);
        }

        private void PlaceFood()
        {
            var rand = new Random();
            Position pos;
            do
            {
                pos = new Position(rand.Next(_cols), rand.Next(_rows));
            } while (_snake.Segments.Contains(pos));

            _foodPosition = pos;
        }

        public void UpdateGridSize(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            // Atualiza posição da comida se estiver fora do novo grid
            if (_foodPosition.X >= _cols || _foodPosition.Y >= _rows)
            {
                PlaceFood();
            }

            // (Opcional) Remover partes da cobra que agora estão fora do novo grid
            _snake.TrimOutside(_cols, _rows);
        }


    }
}
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
        private readonly Snake _snake;
        private Position _foodPosition;
        private int _rows;
        private int _cols;
        private bool _startedByPlayer = false;

        private readonly DispatcherTimer _timer;
        private TimeSpan _interval;
        private int _score = 0;

        // Configuração da dificuldade
        private const int InitialInterval = 500;
        private const int MinInterval = 20;



        public SnakeGameEngine(IGameView view, int rows, int cols, TimeSpan interval)
        {
            _view = view;
            _rows = rows;
            _cols = cols;
            _snake = new Snake(new Position(cols / 2, rows / 2), cols, rows);
            PlaceFood();

            _interval = TimeSpan.FromMilliseconds(InitialInterval);
            _timer = new DispatcherTimer { Interval = _interval };
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

            if (_snake.HasSelfCollision())
            {
                _timer.Stop();
                _view.OnGameOver();
                return;
            }
            if (_snake.Head.Equals(_foodPosition))
            {
                _snake.Grow();
                _score++;
                _view.OnScoreChanged(_score);
                AdjustDifficulty();
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

        private void AdjustDifficulty()
        {
            int currentMs = (int)_interval.TotalMilliseconds;

            if (_score % 1 == 0)
            {
                if (currentMs > 100)
                    currentMs -= 100;
                else if (currentMs > 50)
                    currentMs -= 10;
                else if (currentMs > MinInterval)
                    currentMs -= 1;
            }

            // Atualiza intervalo se mudou
            if (currentMs != (int)_interval.TotalMilliseconds)
            {
                _interval = TimeSpan.FromMilliseconds(currentMs);
                _timer.Interval = _interval;
            }
        }
    }
}
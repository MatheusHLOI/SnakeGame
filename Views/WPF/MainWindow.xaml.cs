using SnakeGameGPT.Engine;
using SnakeGameGPT.Interfaces;
using SnakeGameGPT.Models.Enuns;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace SnakeGameGPT.Views.WPF
{
    public partial class MainWindow : Window, IGameView
    {
        private SnakeGameEngine? _engine;
        private readonly int _cellSize = 20;
        private int _rows;
        private int _cols;
        private int _score = 0;


        public MainWindow()
        {
            InitializeComponent();
            KeyDown += OnKeyDown;

            ScoreText.Text = "Use as setas para começar";
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _rows = (int)(GameCanvas.ActualHeight / _cellSize);
            _cols = (int)(GameCanvas.ActualWidth / _cellSize);

            // Cria nova engine se ainda não existir
            if (_engine == null)
            {
                _engine = new SnakeGameEngine(this, _rows, _cols, TimeSpan.FromMilliseconds(100));
            }
            else
            {
                _engine.UpdateGridSize(_rows, _cols);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_engine == null) return;

            if (e.Key == Key.Up) _engine.ChangeDirection(Direction.Up);
            else if (e.Key == Key.Down) _engine.ChangeDirection(Direction.Down);
            else if (e.Key == Key.Left) _engine.ChangeDirection(Direction.Left);
            else if (e.Key == Key.Right) _engine.ChangeDirection(Direction.Right);
        }

        public void DrawCell(Position position, Brush brush)
        {
            var rect = new Rectangle
            {
                Width = _cellSize,
                Height = _cellSize,
                Fill = brush
            };
            Canvas.SetLeft(rect, position.X * _cellSize);
            Canvas.SetTop(rect, position.Y * _cellSize);
            GameCanvas.Children.Add(rect);
        }

        public void Clear() => GameCanvas.Children.Clear();

        public void UpdateScore()
        {
            _score++;
            ScoreText.Text = $"Pontos: {_score}";
        }

        public void OnScoreChanged(int score)
        {
            _score = score;
            ScoreText.Text = $"Pontos: {_score}";
        }

        public void OnGameStarted()
        {
            ScoreText.Text = "Pontos: 0";
            _score = 0;
        }

        public void OnGameOver()
        {
            ScoreText.Text = $"Game Over! Pontos: {_score}";
            RestartButton.Visibility = Visibility.Visible;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ScoreText.Text = "Use as setas para começar";
            RestartButton.Visibility = Visibility.Collapsed;

            _score = 0;

            _rows = (int)(GameCanvas.ActualHeight / _cellSize);
            _cols = (int)(GameCanvas.ActualWidth / _cellSize);

            _engine = new SnakeGameEngine(this, _rows, _cols, TimeSpan.FromMilliseconds(200));
        }

    }
}


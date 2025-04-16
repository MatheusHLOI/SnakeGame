using System.Windows.Media;
using SnakeGameGPT.Models.Enuns;

namespace SnakeGameGPT.Interfaces
{
    public interface IGameView
    {
        void DrawCell(Position position, Brush brush);
        void Clear();
        void OnScoreChanged(int score);
        void OnGameStarted();


    }
}
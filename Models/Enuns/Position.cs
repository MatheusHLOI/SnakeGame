namespace SnakeGameGPT.Models.Enuns
{
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }
        public Position(int x, int y) => (X, Y) = (x, y);
    }
}
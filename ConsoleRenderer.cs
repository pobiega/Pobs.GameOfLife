using System;
using System.Text;

namespace Pobs.GameOfLife
{
    public interface IRenderer
    {
        void Initialize(int width, int height);
        void Render(BoardState state);
    }

    internal class ConsoleRenderer : IRenderer
    {
        private readonly StringBuilder _builder = new();

        private const char ALIVE = '*';
        private const char DEAD = ' ';

        public void Initialize(int width, int height)
        {
        }

        public void Render(BoardState state)
        {
            for (var y = 0; y < state.Height; y++)
            {
                foreach (var item in state.GetRow(y))
                {
                    _builder.Append(item ? ALIVE : DEAD);
                }

                _builder.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(_builder.ToString());
            _builder.Clear();
        }
    }
}
using Spectre.Console;

namespace Pobs.GameOfLife
{
    internal class SpectreRenderer : IRenderer
    {
        private Table _table;
        private Canvas _canvas;

        public void Initialize(int width, int height)
        {
            _table = new Table();
            _table.AddColumn("Conway's Game of Life");

            _canvas = new Canvas(width, height);
            _table.AddRow(_canvas);
        }

        public void Render(BoardState state)
        {
            for (var y = 0; y < state.Height; y++)
            {
                var row = state.GetRow(y);

                for (var x = 0; x < row.Length; x++)
                {
                    _canvas.SetPixel(x, y, row[x] ? Color.White : Color.Black);
                }
            }

            AnsiConsole.Cursor.SetPosition(0, 0);
            AnsiConsole.Render(_table);
        }
    }
}
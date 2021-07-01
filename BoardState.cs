using System;
using System.IO;

namespace Pobs.GameOfLife
{
    public class BoardState
    {
        public BoardState(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new bool[width * height];
        }

        public int Width { get; }
        public int Height { get; }
        public bool AllCellsDead { get; internal set; }

        private readonly bool[] _cells;

        internal void SetCellState(int x, int y, bool value)
        {
            _cells[(y * Height) + x] = value;
        }

        public (bool, int) GetCellState(int x, int y)
        {
            return (GetValue(x, y), GetNeighbours(x, y));
        }

        private bool GetValue(int idx)
        {
            if (idx < 0 || idx > (Height * Width))
                return false;

            return _cells[idx];
        }

        private bool GetValue(int x, int y) => GetValue((y * Height) + x);

        public ReadOnlySpan<bool> GetRow(int row)
        {
            if (row > Height || row < 0)
            {
                throw new ArgumentException("Specified row was outside of range");
            }

            return _cells.AsSpan(row * Width, Width);
        }

        private int GetNeighbours(int x, int y)
        {
            var count = 0;
            for (var j = -1; j < 1; j++)
            {
                for (var i = -1; i < 1; i++)
                {
                    if (j == 0 && i == 0)
                        continue;

                    if (GetValue(x + i, y + j))
                        count++;
                }
            }

            return count;
        }

        public static BoardState CreateRandomState(int width, int height)
        {
            var random = new Random();
            var state = new BoardState(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    state.SetCellState(x, y, random.Next(0, 3) == 0);
                }
            }

            return state;
        }
        public static BoardState FromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ArgumentException($"No file with name {filename} exists.", nameof(filename));
            }

            var content = File.ReadAllText(filename);
            var rows = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var width = rows[0].Length;
            var height = rows.Length;

            var state = new BoardState(width, height);

            for (var y = 0; y < height; y++)
            {
                var row = rows[y];
                if (row.Length != width)
                {
                    throw new InvalidDataException("Not all rows are of identical length, file is invalid for Game of Life.");

                }
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];

                    if (cell != '_')
                    {
                        state.SetCellState(x, y, true);
                    }
                }
            }

            return state;
        }
    }
}

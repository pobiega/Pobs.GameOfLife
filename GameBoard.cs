using System;
using System.Threading;

namespace Pobs.GameOfLife
{
    public class GameBoard
    {
        private BoardState _state;
        private readonly IRenderer _renderer;

        public GameBoard(IRenderer renderer, int width, int height)
            : this(renderer, BoardState.CreateRandomState(width, height))
        {
        }

        public GameBoard(IRenderer renderer, BoardState initialState)
        {
            _state = initialState;
            _renderer = renderer;
            _renderer.Initialize(initialState.Width, initialState.Height);
        }

        public void Run()
        {
            // timer? keypress? idk

            Render();

            while (!_state.AllCellsDead)
            {
                Thread.Sleep(1000);
                Step();
            }
        }

        public void Step()
        {
            _state = CalculateNextState(_state);
            Render();
        }

        private static BoardState CalculateNextState(BoardState state)
        {
            var newState = new BoardState(state.Width, state.Height);
            var allCellsDead = true;

            for (var y = 0; y < state.Height; y++)
            {
                for (var x = 0; x < state.Width; x++)
                {
                    var (cellState, neighbours) = state.GetCellState(x, y);

                    if (cellState)
                    {
                        allCellsDead = false;
                    }

                    if ((cellState && (neighbours == 2 || neighbours == 3))
                    || (!cellState && neighbours == 3))
                    {
                        // Any live cell with two or three live neighbours survives.
                        // Any dead cell with three live neighbours becomes a live cell.
                        newState.SetCellState(x, y, true);
                    }
                    else
                    {
                        // All other live cells die in the next generation. Similarly, all other dead cells stay dead.
                        newState.SetCellState(x, y, false);
                    }
                }
            }
            newState.AllCellsDead = allCellsDead;

            return newState;
        }

        public void Render()
        {
            _renderer.Render(_state);
        }
    }
}
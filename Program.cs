using Spectre.Console;
using System;
using System.IO;

namespace Pobs.GameOfLife
{
    internal enum Renderers
    {
        Console,
        SpectreConsole
    }

    internal enum StateSource
    {
        Random,
        File
    }

    internal static class Program
    {
        private static void Main()
        {
            var rendererChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Renderers>()
                    .Title("Select your desired renderer.")
                    .AddChoices(Enum.GetValues<Renderers>())
                );

            var stateSource = AnsiConsole.Prompt(
                new SelectionPrompt<StateSource>()
                    .Title("Do you want to randomize a game or load from a file?")
                    .AddChoices(Enum.GetValues<StateSource>())
                );

            IRenderer renderer = rendererChoice switch
            {
                Renderers.Console => new ConsoleRenderer(),
                Renderers.SpectreConsole => new SpectreRenderer(),
            };

            GameBoard gameBoard;

            if (stateSource == StateSource.File)
            {
                var state = BoardState.FromFile(PromptForFilename());
                gameBoard = new GameBoard(renderer, state);
            }
            else
            {
                gameBoard = new GameBoard(renderer, 25, 25);
            }

            gameBoard.Run();
        }

        private static string PromptForFilename()
        {
            var validFiles = Directory.GetFiles("data");

            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select a file from the [teal]/data[/] directory to load as initial state.")
                .AddChoices(validFiles)
            );
        }
    }
}

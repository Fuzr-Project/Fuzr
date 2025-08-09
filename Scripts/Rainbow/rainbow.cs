#!/usr/bin/env dotnet

ConsoleColor[] Rainbow = new[]
{
    ConsoleColor.Red,
    ConsoleColor.Yellow,
    ConsoleColor.Green,
    ConsoleColor.Cyan,
    ConsoleColor.Blue,
    ConsoleColor.Magenta
};

int NextColorIndex(int i) => i % Rainbow.Length;


if (args.Length < 1)
{
    Console.WriteLine("Usage: dotnet run -- \"your text here\"");
    return;
}

// The program uses the first argument only.
string input = args[0];

// Print char-by-char with cycling rainbow colors
for (int i = 0; i < input.Length; i++)
{
    Console.ForegroundColor = Rainbow[NextColorIndex(i)];
    Console.Write(input[i]);
}

// Restore default color and newline
Console.ResetColor();
Console.WriteLine();

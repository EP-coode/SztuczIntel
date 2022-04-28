// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;


Board b = new();

IPlayer player = new ConsolePlayer();

Console.WriteLine(b);

while (true)
{
    Move m = player.MakeMove(b);
    b.MakeMove(m);
    Console.WriteLine(b);
}


Console.WriteLine("Hello, World!");

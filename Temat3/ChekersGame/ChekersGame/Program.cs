// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;


Random rnd = new Random();

Board b = new();

b.MakeMove(new Move(2, 1, 3, 2));
b.MakeMove(new Move(5,0, 4, 1));

Console.WriteLine(b);

var moves = b.GetAllPossibleMoves(3, 2);

while (moves.Count() > 0)
{
    var (row, col) = b.MakeMove(moves[rnd.Next(moves.Count())]);
    moves = b.GetAllPossibleMoves(row, col);
    Console.WriteLine(b);
}



Console.WriteLine("Hello, World!");

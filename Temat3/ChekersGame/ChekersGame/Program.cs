// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;

Board b = new();

Console.WriteLine(b);

var moves = b.GetAllPossibleMoves(2, 1, Piece.BLACK);


Console.WriteLine("Hello, World!");

// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;
using ChekersGame.Chekers.Players;
using System.Collections.Generic;

Dictionary<PieceColor, Player> players = new()
{
    { PieceColor.WHITE, new RandomPlayer(PieceColor.WHITE) },
    { PieceColor.BLACK, new RandomPlayer(PieceColor.BLACK) }
};

Game game = new Game();

while (!game.IsFinished())
{
    Console.WriteLine(game.GameBoard);
    var move = players[game.MovingPlayer].MakeMove(game);
    game.NextMove(move);
}
Console.WriteLine("KONIEC");
Console.WriteLine(game.GameBoard);
// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;
using ChekersGame.Chekers.Players;
using System.Collections.Generic;
using ChekersGame.Chekers.AI;

Dictionary<PieceColor, Player> players = new()
{
    { PieceColor.WHITE, new RandomPlayer(PieceColor.WHITE) },
    { PieceColor.BLACK, new MinimaxAIPlayer(PieceColor.BLACK, 5) }
};

Game game = new Game();

while (!game.IsFinished())
{
    Console.WriteLine(game.GameBoard);
    var move = players[game.MovingPlayer].MakeMove(game);
    Console.WriteLine($"GRACZ {game.MovingPlayer} robi róch: {move}\n");
    game.NextMove(move);
}
Console.WriteLine("KONIEC");
Console.WriteLine($"Przegral {game.MovingPlayer}, tura: {game.Turn}");
Console.WriteLine(game.GameBoard);
// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;
using ChekersGame.Chekers.Players;

Player player1 = new RandomPlayer(PieceColor.WHITE);
Player player2 = new RandomPlayer(PieceColor.BLACK);

Game game = new Game(player1, player2);

while (!game.IsFinished())
{
    Console.WriteLine(game.GameBoard);
    game.NextMove();
}
Console.WriteLine("KONIEC");
// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;

Player player1 = new ConsolePlayer(PieceColor.WHITE);
Player player2 = new ConsolePlayer(PieceColor.BLACK);

Game game = new Game(player1, player2);

while (!game.IsFinished())
{
    Console.WriteLine(game.GameBoard);
    game.NextMove();
}
// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;
using ChekersGame.Chekers.Players;
using ChekersGame.Chekers.AI;
using ChekersGame.MiniMax;


List<(IEvaluator<Board, Move, PieceColor>, double)> evaluators = 
    new List<(IEvaluator<Board, Move, PieceColor>, double)>() { (new BoardPiecesCount(), 1d) };

Dictionary<PieceColor, Player> players = new()
{
    { PieceColor.WHITE, new RandomPlayer(PieceColor.WHITE) },
    { PieceColor.BLACK, new MiniMaxPlayer(PieceColor.BLACK, evaluators, 6) }
};

Game game = new Game();

while (!game.IsFinished())
{
    Console.WriteLine(game.GameBoard);
    var move = players[game.MovingPlayer].MakeMove(game);
    Console.WriteLine($"GRACZ {game.MovingPlayer} robi róch: {move}\n");
    game.ApplyMove(move);
}

Console.WriteLine("KONIEC");
Console.WriteLine($"Przegral {game.MovingPlayer}, tura: {game.Turn}");
Console.WriteLine(game.GameBoard);
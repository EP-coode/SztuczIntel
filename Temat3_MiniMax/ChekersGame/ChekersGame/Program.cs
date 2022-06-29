// See https://aka.ms/new-console-template for more information
using ChekersGame.Chekers;
using ChekersGame;
using ChekersGame.Chekers.Players;
using ChekersGame.Chekers.AI;
using ChekersGame.MiniMax;
using System.Diagnostics;

(int, long, bool) PlayGame(List<(IEvaluator<Board, Move, PieceColor>, double)> evaluators, Dictionary<PieceColor, Player> players, PieceColor player)
{
    Game game = new Game();

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    while (!game.IsFinished())
    {
        Console.WriteLine(game.GameBoard);
        var move = players[game.MovingPlayer].MakeMove(game);
        Console.WriteLine($"GRACZ {game.MovingPlayer} robi róch: {move}\n");
        game.ApplyMove(move);
    }
    stopwatch.Stop();

    return (game.Turn, stopwatch.ElapsedMilliseconds, game.MovingPlayer != player);
}

(int, long, int) PlayGames(List<(IEvaluator<Board, Move, PieceColor>, double)> evaluators, Dictionary<PieceColor, Player> players, int gamesCount, PieceColor pieceColor)
{
    List<int> turnsNumber = new();
    List<long> gameTimes = new();
    int totalWins = 0;

    for(int i = 0; i < gamesCount; i++)
    {
        (int turns, long time, bool winner) = PlayGame(evaluators, players, pieceColor);
        turnsNumber.Add(turns);
        gameTimes.Add(time);
        if (winner)
            totalWins++;
    }

    return ((int)Math.Floor(turnsNumber.Average()), (long)Math.Floor(gameTimes.Average()), totalWins);
}

List<(IEvaluator<Board, Move, PieceColor>, double)> evaluators =
    new List<(IEvaluator<Board, Move, PieceColor>, double)>() {
        (new BoardPiecesCountEvaluator(), 1d)
    };

List<(IEvaluator<Board, Move, PieceColor>, double)> evaluators2 =
    new List<(IEvaluator<Board, Move, PieceColor>, double)>() {
        (new BoardPiecesCountEvaluator(), 1d),
        (new BoardZonesEvaluatorcs(), 0.5d)
    };

Dictionary<PieceColor, Player> playerSet1 = new()
{
    { PieceColor.WHITE, new ConsolePlayer(PieceColor.WHITE) },
    { PieceColor.BLACK, new MiniMaxPlayer(PieceColor.BLACK, evaluators, 3) }
};

Dictionary<PieceColor, Player> playerSet2 = new()
{
    { PieceColor.WHITE, new RandomPlayer(PieceColor.WHITE) },
    { PieceColor.BLACK, new MiniMaxPlayer(PieceColor.BLACK, evaluators, 3, true) }
};

int GAME_COUNT = 20;


Console.WriteLine("Only pieces count evaluator: ");
(int avarageTurnsCount1, long avarageGameTime1, int totalWins1) = PlayGames(evaluators, playerSet1, GAME_COUNT, PieceColor.BLACK);
Console.WriteLine($"MiniMax: avgTurns: {avarageTurnsCount1}, avgTime: {avarageGameTime1}ms, wins:{totalWins1}/{GAME_COUNT}");
(int avarageTurnsCount2, long avarageGameTime2, int totalWins2) = PlayGames(evaluators, playerSet2, GAME_COUNT, PieceColor.BLACK);
Console.WriteLine($"MiniMaxAlphaBeta: avgTurns: {avarageTurnsCount2}, avgTime: {avarageGameTime2}ms, wins:{totalWins2}/{GAME_COUNT}");

Console.WriteLine("Pieces count evaluator + Board zones Evaluator");
(int avarageTurnsCount3, long avarageGameTime3, int totalWins3) = PlayGames(evaluators2, playerSet1, GAME_COUNT, PieceColor.BLACK);
Console.WriteLine($"MiniMax: avgTurns: {avarageTurnsCount3}, avgTime: {avarageGameTime3}ms, wins:{totalWins3}/{GAME_COUNT}");
(int avarageTurnsCount4, long avarageGameTime4, int totalWins4) = PlayGames(evaluators2, playerSet2, GAME_COUNT, PieceColor.BLACK);
Console.WriteLine($"MiniMaxAlphaBeta: avgTurns: {avarageGameTime4}, avgTime: {avarageGameTime4}ms, wins:{totalWins4}/{GAME_COUNT}");
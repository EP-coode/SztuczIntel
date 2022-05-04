using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChekersGame.Chekers;

namespace ChekersGame.Chekers.AI;

public class MinimaxAIPlayer : Player
{
    public int MaxDepth { get; set; }
    private IBoardEvaluator boardEvaluator;

    public MinimaxAIPlayer(PieceColor color, int maxDepth = 3) : base(color)
    {
        MaxDepth = maxDepth;
        boardEvaluator = new BoardPiecesCount();
    }

    public override Move MakeMove(Game g)
    {
        return MiniMax(g, MaxDepth, 0, 1, true).Item2;
    }

    private (double, Move?) MiniMax(Game g, int depth, double alpha, double beta, bool maximizingPlayer)
    {
        var baoard = g.GameBoard;

        if (depth == 0 || g.IsFinished())
            return (boardEvaluator.Evaluate(g.GameBoard, PlayerPieceColor), null);

        if (maximizingPlayer)
        {
            double maxEval = 0;
            List<Move> moves = baoard.GetAllPossibleMoves(g.MovingPlayer);
            Move bestMove = moves[0];

            foreach (var move in moves)
            {
                Game gameClone = new Game(g);
                gameClone.NextMove(move);
                var (moveEvaluation, _) = MiniMax(gameClone, depth - 1, alpha, beta, false);

                if (moveEvaluation > maxEval)
                {
                    maxEval = moveEvaluation;
                    bestMove = move;
                }
                alpha = Math.Max(alpha, maxEval);

                if (maxEval >= beta)
                    break;
            }

            return (maxEval, bestMove);
        }
        else
        {
            double minEval = 1;
            List<Move> moves = baoard.GetAllPossibleMoves(g.MovingPlayer);
            Move worstMove = moves[0];

            foreach (var move in moves)
            {
                Game gameClone = new Game(g);
                gameClone.NextMove(move);
                var (moveEvaluation, _) = MiniMax(gameClone, depth - 1, alpha, beta, true);
                if (moveEvaluation < minEval)
                {
                    minEval = moveEvaluation;
                    worstMove = move;
                }

                beta = Math.Max(beta, minEval);

                if (minEval <= alpha)
                    break;

            }

            return (minEval, worstMove);
        }
    }
}


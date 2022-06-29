using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.MiniMax;

public class MiniMaxAlphaBeta<T, M, P> : IMiniMax<T, M, P>
{
    private List<(IEvaluator<T, M, P>, double)> evaluatorsAndWeights;
    public int MaxDepth { get; set; }

    public MiniMaxAlphaBeta(List<(IEvaluator<T, M, P>, double)> evaluatorsAndWeights, int maxDepth)
    {
        this.evaluatorsAndWeights = evaluatorsAndWeights;
        MaxDepth = maxDepth;
    }

    public M? GetBestMove(IMiniMaxGame<T, M, P> game)
    {
        P player = game.GetCurrentPlayer();
        return MiniMaxAlghoritm(game, MaxDepth, 0, 1, player, true).Item2;
    }

    private (double, M?) MiniMaxAlghoritm(IMiniMaxGame<T, M, P> g, int depth, double alpha, double beta, P player, bool maximizingPlayer)
    {
        double stateValue = 0;

        foreach (var (evaluator, weight) in evaluatorsAndWeights)
        {
            stateValue += evaluator.EvaluateFromPlayerPErspective(g, player) * weight;
        }

        if (depth == 0 || g.IsFinished())
            return (stateValue, default(M));

        if (maximizingPlayer)
        {
            double maxEval = 0;
            List<M> moves = g.GetMoves();
            M bestMove = moves[0];

            foreach (var move in moves)
            {
                IMiniMaxGame<T, M, P> gameClone = g.Clone();
                gameClone.ApplyMove(move);
                var (moveEvaluation, _) = MiniMaxAlghoritm(gameClone, depth - 1, alpha, beta, player, false);

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
            List<M> moves = g.GetMoves();
            M worstMove = moves[0];

            foreach (var move in moves)
            {
                IMiniMaxGame<T, M, P> gameClone = g.Clone();
                gameClone.ApplyMove(move);
                var (moveEvaluation, _) = MiniMaxAlghoritm(gameClone, depth - 1, alpha, beta, player, true);

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


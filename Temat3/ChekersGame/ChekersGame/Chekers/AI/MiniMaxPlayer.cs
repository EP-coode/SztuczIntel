using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChekersGame.MiniMax;
using ChekersGame.Chekers;

namespace ChekersGame.Chekers.AI;

public class MiniMaxPlayer : Player
{
    private MiniMax<Board, Move, PieceColor> miniMax;

    public MiniMaxPlayer(
        PieceColor color,
        List<(IEvaluator<Board, Move, PieceColor>, double)> boardEvaluatorsAndWeights,
        int maxDepth = 3)
        : base(color)
    {
        miniMax = new MiniMax<Board, Move, PieceColor>(boardEvaluatorsAndWeights, maxDepth);
    }

    public override Move MakeMove(Game g)
    {
        return miniMax.GetBestMove(g);
    }

}


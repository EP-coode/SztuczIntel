using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChekersGame.Chekers;

namespace ChekersGame.Chekers.AI;

public class MinimaxAIPlayer : Player
{
    public int MaxDepth { get; set; } = 10;
    public MinimaxAIPlayer(PieceColor color) : base(color)
    {
    }

    public override Move MakeMove(Board b)
    {
        throw new NotImplementedException();
    }

    private (Move, float) MiniMax(Board g, int depth, bool maxPlayer)
    {
        throw new NotImplementedException();
    }
}


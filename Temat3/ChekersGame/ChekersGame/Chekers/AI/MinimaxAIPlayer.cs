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

    //private float MiniMax(Board board, int depth, bool maximizingPlayer)
    //{
    //    if(depth == 0 || board)
    //}
}


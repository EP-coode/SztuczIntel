using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers.AI;

internal interface IBoardEvaluator
{
    public double Evaluate(Board board, Player player);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers.AI;

internal interface IBoardEvaluator
{
    /// <summary>
    /// Returns value betwen [0,1] 
    /// </summary>
    /// <param name="board"></param>
    /// <param name="playerPieceColor"></param>
    /// <returns></returns>
    public double Evaluate(Board board, PieceColor playerPieceColor);
}


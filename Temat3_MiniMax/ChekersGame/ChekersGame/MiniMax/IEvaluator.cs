using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.MiniMax;

// TODO improve generalization 
public interface IEvaluator<T, M, P> 
{
    public double EvaluateFromPlayerPErspective(IMiniMaxGame<T, M, P> game, P player);
}


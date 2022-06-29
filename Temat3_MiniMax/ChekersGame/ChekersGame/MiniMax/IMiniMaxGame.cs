using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.MiniMax;

public interface IMiniMaxGame<T,M, P>
{
    public IMiniMaxGame<T, M, P> Clone();
    public bool IsFinished();
    public List<M> GetMoves();
    public T GetState();
    public void ApplyMove(M move);
    public P GetCurrentPlayer();
}


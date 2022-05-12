namespace ChekersGame.MiniMax;

public interface IMiniMax<T, M, P>
{
    public int MaxDepth { get; set; }

    public M? GetBestMove(IMiniMaxGame<T, M, P> game);
}

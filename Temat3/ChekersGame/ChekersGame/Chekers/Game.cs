using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChekersGame.MiniMax;

namespace ChekersGame.Chekers;

public class Game : IMiniMaxGame<Board, Move, PieceColor>
{
    public const int MAX_NO_OF_KINGS_MOVES = 15;

    private PieceColor _movingPlayer;
    private List<Move> _currentPlayerAvalibleMoves;
    private Board _gameBoard;
    private int _turns;
    private int _lastNonIddleNonKingPieceMove;

    public int Turn { get { return _turns; } }

    public PieceColor MovingPlayer { get { return _movingPlayer; } }
    public Board GameBoard { get { return _gameBoard; } }

    public Game()
    {
        _movingPlayer = PieceColor.WHITE;
        _gameBoard = new Board();
        _turns = 0;
        _lastNonIddleNonKingPieceMove = 0;
        _currentPlayerAvalibleMoves = GameBoard.GetAllPossibleMoves(MovingPlayer);
    }

    public Game(Game g)
    {
        _movingPlayer = g.MovingPlayer;
        _gameBoard = new Board(g.GameBoard);
        _turns = g._turns;
        _lastNonIddleNonKingPieceMove = g._lastNonIddleNonKingPieceMove;
        _currentPlayerAvalibleMoves = g._currentPlayerAvalibleMoves;
    }

    public void ApplyMove(Move playerMove)
    {
        if (IsFinished())
            throw new InvalidOperationException("This game is finished");

        bool isMoveValid = _currentPlayerAvalibleMoves.Any(move => playerMove.Equals(move));

        if (isMoveValid)
        {
            var (row, col, _, _) = playerMove;
            Piece? movingPiece = GameBoard[row, col];
            if (movingPiece != null && !movingPiece.IsKing && !playerMove.IsCapture)
            {
                _lastNonIddleNonKingPieceMove = _turns;
            }

            _gameBoard = GameBoard.MakeMove(playerMove);
            _turns++;
        }
        else
        {
            throw new InvalidOperationException("This move is not allowed in game");
        }

        SwapPlayers();
        _currentPlayerAvalibleMoves = GameBoard.GetAllPossibleMoves(MovingPlayer);
    }

    public bool IsFinished()
    {
        if (_currentPlayerAvalibleMoves.Count() == 0)
            return true;

        int piecesCount = GameBoard.GetPieces(MovingPlayer).Count();

        if (piecesCount <= 0)
            return true;

        if (_turns - _lastNonIddleNonKingPieceMove > MAX_NO_OF_KINGS_MOVES)
            return true;


        return false;
    }

    private void SwapPlayers()
    {
        if (MovingPlayer == PieceColor.WHITE)
            _movingPlayer = PieceColor.BLACK;
        else
            _movingPlayer = PieceColor.WHITE;
    }

    public IMiniMaxGame<Board, Move, PieceColor> Clone()
    {
        return new Game(this);
    }

    public List<Move> GetMoves()
    {
        return _currentPlayerAvalibleMoves;
    }

    public Board GetState()
    {
        return GameBoard;
    }

    public PieceColor GetCurrentPlayer()
    {
        return MovingPlayer;
    }
}

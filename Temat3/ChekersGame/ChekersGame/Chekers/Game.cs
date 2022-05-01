using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public class Game
{
    public const int MAX_NO_OF_KINGS_MOVES = 10;

    private Player _movingPlayer;
    private List<Move> _currentPlayerAvalibleMoves;
    private Board _gameBoard;
    private int _turns;
    private int _lastNonIddleNonKingPieceMove;

    public Player MovingPlayer { get { return _movingPlayer; } }
    public Board GameBoard { get { return _gameBoard; } }
    public Player WhitePlayer { get; }
    public Player BlackPlayer { get; }

    public Game(Player whitePlayer, Player blackPlayer)
    {
        WhitePlayer = whitePlayer;
        BlackPlayer = blackPlayer;
        _movingPlayer = whitePlayer;
        _gameBoard = new Board();
        _turns = 0;
        _lastNonIddleNonKingPieceMove = 0;
        _currentPlayerAvalibleMoves = GameBoard.GetAllPossibleMoves(MovingPlayer.PlayerPieceColor);
    }

    public void NextMove()
    {
        if (IsFinished())
            throw new InvalidOperationException("This game is finished");

        Move playerMove = MovingPlayer.MakeMove(GameBoard);

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
        _currentPlayerAvalibleMoves = GameBoard.GetAllPossibleMoves(MovingPlayer.PlayerPieceColor);
    }

    public bool IsFinished()
    {
        if (_currentPlayerAvalibleMoves.Count() == 0)
            return true;

        int piecesCount = GameBoard.GetPieces(MovingPlayer.PlayerPieceColor).Count();

        if (piecesCount <= 0)
            return true;

        if (_turns - _lastNonIddleNonKingPieceMove > MAX_NO_OF_KINGS_MOVES)
            return true;


        return false;
    }

    private void SwapPlayers()
    {
        if (MovingPlayer == WhitePlayer)
            _movingPlayer = BlackPlayer;
        else
            _movingPlayer = WhitePlayer;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public class Board
{
    public const int BOARD_SIZE = 8;
    private Piece?[,] boardBlackTiles;

    public Board()
    {
        // [row, col]
        // squished verticaly
        boardBlackTiles = new Piece[BOARD_SIZE / 2, BOARD_SIZE];
        InitOrResetBoard();
    }

    public Board(Board board)
    {
        boardBlackTiles = new Piece[BOARD_SIZE / 2, BOARD_SIZE];
        for (int i = 0; i < boardBlackTiles.GetLength(0); i++)
        {
            for (int j = 0; j < boardBlackTiles.GetLength(1); j++)
            {
                var pieceAtPosition = board.boardBlackTiles[i, j];
                if (pieceAtPosition != null)
                    boardBlackTiles[i, j] = new Piece(pieceAtPosition);
                else
                    boardBlackTiles[i, j] = null;
            }
        }
    }

    public Piece? this[int row, int col]
    {
        get
        {
            if (IsWhiteTile(row, col))
            {
                return null;
            }

            return boardBlackTiles[row / 2, col];
        }
        set
        {
            if (IsWhiteTile(row, col))
            {
                throw new InvalidOperationException("You can put pieces only on black fields");
            }

            boardBlackTiles[row / 2, col] = value;
        }
    }

    public Board MakeMove(Move m)
    {
        var (src_row, src_col, dst_row, dst_col) = m;

        if (this[src_row, src_col] == null)
            throw new InvalidOperationException("Can't move non existing piece");

        Board boardClone = new Board(this);

        Piece movingPiece = boardClone[src_row, src_col];

        boardClone[src_row, src_col] = null;


        if (m.IsCapture)
        {
            int dy = src_row < dst_row ? -1 : 1;
            int dx = src_col < dst_col ? -1 : 1;
            boardClone[dst_row + dy, dst_col + dx] = null;
        }

        if (m.NextMove is not null)
            MakeMove(m.NextMove, boardClone, movingPiece);
        else
        {
            boardClone[dst_row, dst_col] = movingPiece;
            movingPiece.SetPosition(dst_row, dst_col);
            if (CanBecameKing(movingPiece))
                movingPiece.IsKing = true;
        }

        return boardClone;
    }

    private static void MakeMove(Move m, Board board, Piece movingPiece)
    {
        var (src_row, src_col, dst_row, dst_col) = m;
        if (m.IsCapture)
        {
            int dy = src_row < dst_row ? -1 : 1;
            int dx = src_col < dst_col ? -1 : 1;
            board[dst_row + dy, dst_col + dx] = null;
        }


        if (m.NextMove is not null)
            MakeMove(m.NextMove, board, movingPiece);
        else
        {
            board[dst_row, dst_col] = movingPiece;
            movingPiece.SetPosition(dst_row, dst_col);
            if (CanBecameKing(movingPiece))
                movingPiece.IsKing = true;
        }
    }

    public List<Move> GetAllPossibleMoves(Piece p)
    {
        Piece? movingPiece = this[p.Row, p.Col];

        if (movingPiece == null)
            throw new InvalidOperationException("In this position there is no piece");

        return GetAllPossibleMoves(p.Row, p.Col, movingPiece, this);
    }

    public List<Move> GetAllPossibleMoves(PieceColor color)
    {
        List<Piece> pieces = GetPieces(color);
        List<Move> avalibleMoves = new List<Move>();

        foreach (Piece piece in pieces)
        {
            avalibleMoves.AddRange(GetAllPossibleMoves(piece));
        }

        return avalibleMoves;
    }

    public List<Piece> GetPieces(PieceColor color)
    {
        List<Piece> pieces = new List<Piece>();
        foreach (Piece? piece in boardBlackTiles)
        {
            if (piece != null && piece.PieceColor == color)
                pieces.Add(piece);
        }
        return pieces;
    }

    public List<Piece> GetPieces()
    {
        List<Piece> pieces = new List<Piece>();
        foreach (Piece? piece in boardBlackTiles)
        {
            if (piece != null)
                pieces.Add(piece);
        }
        return pieces;
    }
    private static List<Move> GetAllPossibleMoves(int src_row, int src_col, Piece movingPiece, Board board, bool onlyCaptures = false, int depth = 0)
    {
        List<Move> possibleMoves = new List<Move>();
        List<Move> captures;

        if (movingPiece.IsKing)
            captures = board.GetCapturesForKing(src_row, src_col, movingPiece);
        else
            captures = board.GetCapturesForPiece(src_row, src_col, movingPiece);

        if (captures.Count() > 0)
        {
            foreach (Move move in captures)
            {
                int finalRow = move.FinalRow;
                int finalCol = move.FinalCol;

                var boardNewState = board.MakeMove(move);
                var newMovingPiece = boardNewState[finalRow, finalCol];
                var nextMoves = GetAllPossibleMoves(finalRow, finalCol, newMovingPiece, boardNewState, true, depth + 1);

                foreach (Move nextMove in nextMoves)
                {
                    var moveClone = new Move(move);
                    moveClone.NextMove = nextMove;
                    possibleMoves.Add(moveClone);
                }
                if (nextMoves.Count() == 0)
                {
                    possibleMoves.Add(move);
                }
            }
        }
        else if (!onlyCaptures)
        {
            if (movingPiece.IsKing)
                possibleMoves.AddRange(board.GetAllJumpsForKing(src_row, src_col, movingPiece));
            else
                possibleMoves.AddRange(board.GetAllJumpsForPiece(src_row, src_col, movingPiece));
        }

        return possibleMoves;
    }

    private List<Move> GetAllJumpsForPiece(int src_row, int src_col, Piece movingPiece)
    {
        if (movingPiece.IsKing)
            throw new InvalidCastException("You can call this method only for normal pieces");

        List<(int, int)> jumpDirections;
        List<Move> possibleMoves = new List<Move>();

        if (movingPiece.PieceColor == PieceColor.BLACK)
        {
            jumpDirections = new()
            {
                (1, 1),
                (1, -1)
            };
        }
        else
        {
            jumpDirections = new()
            {
                (-1, 1),
                (-1, -1)
            };
        }

        // filter jump directions that leads out of the board
        jumpDirections = jumpDirections.FindAll(x => IsPositionInBoard(x.Item1 + src_row, x.Item2 + src_col));

        foreach (var (dy, dx) in jumpDirections)
        {
            int dst_col = src_col + dx;
            int dst_row = src_row + dy;
            Piece? pieceAtDestination = this[dst_row, dst_col];

            if (pieceAtDestination == null)
                possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col));
        }
        return possibleMoves;
    }

    private List<Move> GetCapturesForPiece(int src_row, int src_col, Piece movingPiece)
    {
        if (movingPiece.IsKing)
            throw new InvalidCastException("You can call this method only for normal pieces");

        List<(int, int)> jumpDirections = new()
        {
            (-2, -2),
            (2, 2),
            (2, -2),
            (-2, 2),
        };
        List<Move> possibleMoves = new List<Move>();

        jumpDirections = jumpDirections.FindAll(x => IsPositionInBoard(x.Item1 + src_row, x.Item2 + src_col)).ToList();

        foreach (var (dy, dx) in jumpDirections)
        {
            int dst_col = src_col + dx;
            int dst_row = src_row + dy;

            Piece? pieceAtDestination = this[dst_row, dst_col];
            Piece? capturedPiece = this[dst_row - dy / 2, dst_col - dx / 2];

            if (pieceAtDestination == null && capturedPiece != null && HasOppositeColor(capturedPiece, movingPiece))
                possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col, true));

        }

        return possibleMoves;
    }

    private List<Move> GetAllJumpsForKing(int src_row, int src_col, Piece movingPiece)
    {
        if (!movingPiece.IsKing)
            throw new InvalidCastException("You can call this method only for kings");

        List<(int, int)> directions = new()
        {
            (-1, -1),
            (1, 1),
            (1, -1),
            (-1, 1),
        };

        List<Move> possibleMoves = new List<Move>();

        foreach (var (dy, dx) in directions)
        {
            int dst_col = src_col;
            int dst_row = src_row;

            while (IsPositionInBoard(dst_row + dy, dst_col + dx))
            {
                dst_row += dy;
                dst_col += dx;
                Piece? pieceAtDestination = this[dst_row, dst_col];

                if (pieceAtDestination == null)
                    possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col));
                else
                    break;
            }
        }

        return possibleMoves;
    }

    private List<Move> GetCapturesForKing(int src_row, int src_col, Piece movingPiece)
    {
        if (!movingPiece.IsKing)
            throw new InvalidCastException("You can call this method only for kings");

        List<(int, int)> directions = new()
        {
            (-1, -1),
            (1, 1),
            (1, -1),
            (-1, 1),
        };

        List<Move> possibleMoves = new List<Move>();

        foreach (var (dy, dx) in directions)
        {
            bool oponentOccured = false;
            int dst_col = src_col;
            int dst_row = src_row;

            while (IsPositionInBoard(dst_row + dy, dst_col + dx))
            {
                dst_row += dy;
                dst_col += dx;
                Piece? pieceAtDestination = this[dst_row, dst_col];

                if (oponentOccured)
                {
                    if (pieceAtDestination == null)
                        possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col, true));
                    break;
                }
                else
                {
                    if (pieceAtDestination == null)
                        continue;
                    else
                    {
                        if (HasOppositeColor(pieceAtDestination, movingPiece))
                            oponentOccured = true;
                        else
                            // friend occured
                            break;
                    }
                }
            }
        }

        return possibleMoves;
    }

    private bool IsPositionInBoard(int row, int col)
    {
        return (col >= 0 && col < BOARD_SIZE && row >= 0 && row < BOARD_SIZE);
    }

    private static bool HasOppositeColor(Piece p1, Piece p2)
    {
        return p1.PieceColor == PieceColor.WHITE && p2.PieceColor == PieceColor.BLACK || p2.PieceColor == PieceColor.WHITE && p1.PieceColor == PieceColor.BLACK;
    }

    private void InitOrResetBoard()
    {
        // iterate over black fields
        // rows
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            // cols
            for (
                int j = i % 2 == 0 ? 1 : 0;
                j < BOARD_SIZE;
                j += 2)
            {
                if (i < 3)
                    this[i, j] = new Piece(i, j, PieceColor.BLACK);
                else if (i > 4)
                    this[i, j] = new Piece(i, j, PieceColor.WHITE);
                else
                    this[i, j] = null;
            }
        }
    }

    private bool IsWhiteTile(int row, int col)
    {
        if (!IsPositionInBoard(row, col))
            throw new InvalidOperationException("this position do not exist on the board");

        return col % 2 == 0 && row % 2 == 0 || col % 2 == 1 && row % 2 == 1;
    }

    private static bool CanBecameKing(Piece p)
    {
        if (p.IsKing)
            return false;

        switch (p.PieceColor)
        {
            case PieceColor.BLACK:
                return p.Row == 7;
            case PieceColor.WHITE:
                return p.Row == 0;
            default:
                return false;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            sb.Append($"{i}|");
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                Piece? movingPiece = this[i, j];

                if (movingPiece == null)
                    sb.Append('-');
                else if (movingPiece.PieceColor == PieceColor.WHITE)
                    sb.Append(movingPiece.IsKing ? 'W' : 'w');
                else if (movingPiece.PieceColor == PieceColor.BLACK)
                    sb.Append(movingPiece.IsKing ? 'B' : 'b');
                else
                {
                    if (IsWhiteTile(i, j))
                        sb.Append('~');
                    else
                        sb.Append('-');
                }
            }
            sb.AppendLine();
        }
        sb.AppendLine("--01234567");

        return sb.ToString();
    }
}


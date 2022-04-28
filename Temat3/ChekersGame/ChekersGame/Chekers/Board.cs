using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public enum Piece
{
    WHITE,
    WHITE_KING,
    BLACK,
    BLACK_KING,
    EMPTY,
}

public class Board
{
    public const int BOARD_SIZE = 8;
    private Piece[,] board;

    public Board()
    {
        // [row, col]
        // squished verticaly
        board = new Piece[BOARD_SIZE / 2, BOARD_SIZE];
        InitOrResetBoard();
    }

    public Board(Board board)
    {
        throw new NotImplementedException();
    }

    public Piece this[int row, int col]
    {
        get
        {
            if (!IsBlackField(row, col))
            {
                return Piece.EMPTY;
            }

            return board[row / 2, col];
        }
        set
        {
            if (!IsBlackField(row, col))
            {
                throw new InvalidOperationException("You can put pieces only on black fields");
            }

            board[row / 2, col] = value;
        }
    }

    public bool IsBlackField(int row, int col)
    {
        return (row % 2 == 1 && col % 2 == 0) || (row % 2 == 0 && col % 2 == 1);
    }

    public (int, int) MakeMove(Move m)
    {
        var (src_row, src_col, dst_row, dst_col) = m;

        Piece movingPiece = this[src_row, src_col];
        this[src_row, src_col] = Piece.EMPTY;

        if (m.IsCapture)
        {
            int dy = src_row - dst_row > 0 ? -1 : 1;
            int dx = src_col - dst_col > 0 ? -1 : 1;
            this[dst_row - dy, dst_col - dx] = Piece.EMPTY;
        }

        if (m.NextMove is not null)
            return MakeMove(m.NextMove, movingPiece);
        else
        {
            this[dst_row, dst_col] = movingPiece;
            return (dst_row, dst_col);
        }
    }

    private (int, int) MakeMove(Move m, Piece movingPiece)
    {
        var (src_row, src_col, dst_row, dst_col) = m;
        if (m.IsCapture)
        {
            int dy = src_row - dst_row > 0 ? 1 : -1;
            int dx = src_col - dst_col > 0 ? 1 : -1;
            this[dst_row - dy, src_col - dx] = Piece.EMPTY;
        }

        if (m.NextMove is not null)
            return MakeMove(m.NextMove, movingPiece);
        else
        {
            this[dst_row, dst_col] = movingPiece;
            return (dst_row, dst_col);
        }
    }

    public List<Move> GetAllPossibleMoves(int src_row, int src_col)
    {
        return GetAllPossibleMoves(src_row, src_col, this[src_row, src_col]);
    }

    private List<Move> GetAllPossibleMoves(int src_row, int src_col, Piece movingPiece)
    {
        if (movingPiece == Piece.EMPTY)
            throw new InvalidOperationException("You can't move empty piece");

        List<Move> possibleMoves = new List<Move>();

        // TODO - remove redundant code
        if (IsKing(movingPiece))
        {
            var kingCaptures = GetCapturesForKing(src_row, src_col);
            if (kingCaptures.Count() > 0)
            {
                foreach (Move move in kingCaptures)
                {
                    var (_, _, dst_row, dst_col) = move;
                    var nextMoves = GetAllPossibleMoves(dst_row, dst_col, movingPiece);
                    foreach (Move nextMove in nextMoves)
                    {
                        move.NextMove = nextMove;
                    }
                }
                possibleMoves.AddRange(kingCaptures);
            }
            else
            {
                possibleMoves.AddRange(GetAllJumpsForKing(src_row, src_col, movingPiece));
            }
        }
        else
        {
            var pieceCaptures = GetCapturesForPiece(src_row, src_col, movingPiece);
            if (pieceCaptures.Count() > 0)
            {
                foreach (Move move in pieceCaptures)
                {
                    var (_, _, dst_row, dst_col) = move;
                    var nextMoves = GetAllPossibleMoves(dst_row, dst_col, movingPiece);
                    foreach (Move nextMove in nextMoves)
                    {
                        move.NextMove = nextMove;
                    }
                }
                possibleMoves.AddRange(pieceCaptures);
            }
            else
            {
                possibleMoves.AddRange(GetAllJumpsForPiece(src_row, src_col, movingPiece));
            }
        }

        return possibleMoves;
    }

    private List<Move> GetAllJumpsForKing(int src_row, int src_col, Piece movingPiece)
    {
        if (!IsKing(movingPiece))
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
            while (dst_col > 0 && dst_row > 0)
            {
                dst_row += dx;
                dst_col += dy;
                Piece pieceAtDestination = this[dst_row, dst_col];

                if (pieceAtDestination == Piece.EMPTY)
                    possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col));
                else
                    break;
            }
        }

        return possibleMoves;
    }

    private List<Move> GetAllJumpsForPiece(int src_row, int src_col, Piece movingPiece)
    {
        if (movingPiece != Piece.BLACK && movingPiece != Piece.WHITE)
            throw new InvalidCastException("You can call this method only for normal pieces");

        List<(int, int)> directions;

        if (this[src_row, src_col] == Piece.BLACK)
        {
            directions = new()
            {
                (1, 1),
                (1, -1)
            };
        }
        else
        {
            directions = new()
            {
                (-1, 1),
                (-1, -1)
            };
        }

        List<Move> possibleMoves = new List<Move>();
        directions = directions.FindAll(x => IsPositionInBoard(x.Item1 + src_row, x.Item2 + src_col));
        foreach (var (dy, dx) in directions)
        {
            int dst_col = src_col + dx;
            int dst_row = src_row + dy;
            Piece pieceAtDestination = this[dst_row, dst_col];

            if (pieceAtDestination == Piece.EMPTY)
                possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col));
        }
        return possibleMoves;
    }

    private List<Move> GetCapturesForPiece(int src_row, int src_col, Piece movingPiece)
    {
        if (movingPiece != Piece.BLACK && movingPiece != Piece.WHITE)
            throw new InvalidCastException("You can call this method only for normal pieces");

        List<(int, int)> moves = new()
        {
            (-2, -2),
            (2, 2),
            (2, -2),
            (-2, 2),
        };

        moves = moves.FindAll(x => IsPositionInBoard(x.Item1 + src_row, x.Item2 + src_col)).ToList();

        List<Move> possibleMoves = new List<Move>();

        foreach (var (dy, dx) in moves)
        {
            int dst_col = src_col + dx;
            int dst_row = src_row + dy;

            Piece pieceAtDestination = this[dst_row, dst_col];
            Piece capturedPiece = this[dst_row - dy / 2, dst_col - dx / 2];

            if (pieceAtDestination == Piece.EMPTY && HasOppositeColor(capturedPiece, movingPiece))
                possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col, true));

        }

        return possibleMoves;
    }

    private List<Move> GetCapturesForKing(int src_row, int src_col)
    {
        if (!IsKing(this[src_row, src_col]))
            throw new InvalidCastException("You can call this method only for kings");

        List<(int, int)> directions = new()
        {
            (-1, -1),
            (1, 1),
            (1, -1),
            (-1, 1),
        };

        Piece movingPiece = this[src_row, src_col];

        List<Move> possibleMoves = new List<Move>();

        foreach (var (dx, dy) in directions)
        {
            bool pieceOccured = false;
            int dst_col = src_col;
            int dst_row = src_row;
            while (dst_col > 0 && dst_row > 0)
            {
                dst_row += dx;
                dst_col += dy;
                Piece pieceAtDestination = this[dst_row, dst_col];

                if (!pieceOccured)
                {

                    if (HasOppositeColor(pieceAtDestination, movingPiece))
                        pieceOccured = true;

                    else
                        break;
                }
                else
                {
                    if (pieceAtDestination == Piece.EMPTY)
                        possibleMoves.Add(new Move(src_row, src_col, dst_row, dst_col, true));

                    break;
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
        return IsPieceWhite(p1) && IsPieceBlack(p2) || IsPieceBlack(p1) && IsPieceWhite(p2);
    }

    private static bool IsKing(Piece p)
    {
        return p == Piece.BLACK_KING || p == Piece.WHITE_KING;
    }

    private static bool IsPieceWhite(Piece p)
    {
        return p == Piece.WHITE || p == Piece.WHITE_KING;
    }

    private static bool IsPieceBlack(Piece p)
    {
        return p == Piece.BLACK || p == Piece.BLACK_KING;
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
                    this[i, j] = Piece.BLACK;
                else if (i > 4)
                    this[i, j] = Piece.WHITE;
                else
                    this[i, j] = Piece.EMPTY;
            }
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
                Piece movingPiece = this[i, j];

                switch (movingPiece)
                {
                    case Piece.BLACK:
                        sb.Append('b');
                        break;
                    case Piece.BLACK_KING:
                        sb.Append('B');
                        break;
                    case Piece.WHITE:
                        sb.Append('w');
                        break;
                    case Piece.WHITE_KING:
                        sb.Append('W');
                        break;
                    case Piece.EMPTY:
                        sb.Append('-');
                        break;
                    default:
                        sb.Append('~');
                        break;
                }

            }
            sb.AppendLine();
        }
        sb.AppendLine("--01234567");

        return sb.ToString();
    }
}


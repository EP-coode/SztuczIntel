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

    //private List<(int, int)> GetAllPossibleMoves(int src_row, int src_col)
    //{
    //    if (src_col < 0 || src_col > BOARD_SIZE || src_row < 0 || src_row > BOARD_SIZE)
    //        throw new InvalidOperationException();

    //    Piece srcPiece = this[src_row, src_col];

    //    if (srcPiece.Color == PieceColor.NONE)
    //        throw new InvalidOperationException("You can't move from empty space!");


    //}

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

        return sb.ToString();
    }
}


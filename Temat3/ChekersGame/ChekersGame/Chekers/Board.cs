using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public enum Piece
{
    EMPTY = 0,
    BLACK = 1,
    BLACK_KING = 2,
    WHITE = 3,
    WHITE_KING = 4,
}


public class Board
{
    public const int BOARD_SIZE = 8;
    private Piece[,] board;

    public Board()
    {
        // [row, col]
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
                switch (this[i, j])
                {
                    case Piece.BLACK:
                        sb.Append('b');
                        break;
                    case Piece.WHITE:
                        sb.Append('w');
                        break;
                    default:
                        sb.Append('-');
                        break;
                }
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}


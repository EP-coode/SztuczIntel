using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public enum PieceColor
{
    WHITE,
    BLACK,
}

public class Piece
{
    private int row;
    private int col;
    private PieceColor pieceColor;

    public PieceColor PieceColor { get { return pieceColor; } }
    public bool IsKing { get; set; }
    public int Row { get { return row; } }
    public int Col { get { return col; } }


    public Piece(int row, int col, PieceColor color, bool isKing = false)
    {
        this.row = row;
        this.col = col;
        pieceColor = color;
        IsKing = isKing;
    }

    public Piece(Piece p)
    {
        row = p.row;
        col = p.col;
        pieceColor = p.PieceColor;
        IsKing = p.IsKing;
    }

    public void SetPosition(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public override string ToString()
    {
        string result = "";

        if (pieceColor == PieceColor.WHITE)
            result += "WHITE";
        else
            result += "BLACK";

        if (IsKing)
            result += " KING";
        else
            result += " PAWN";

        result += $" ({row},{col})";

        return result;
    }
}


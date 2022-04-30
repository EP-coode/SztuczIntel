using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public class Move
{
    private int src_row, src_col;
    private int dest_row, dest_col;
    public Move? NextMove { get; set; }
    public bool IsCapture { get; set; }

    public void Deconstruct(out int src_row, out int src_col, out int dst_row, out int dst_col)
    {
        src_row = this.src_row;
        src_col = this.src_col;    
        dst_row = this.dest_row;
        dst_col = this.dest_col;
    }

    public Move(int src_row, int src_col, int dst_row, int dst_col, bool isCapture = false)
    {
        this.src_row = src_row;
        this.src_col = src_col;
        this.dest_row = dst_row;
        this.dest_col = dst_col;
        this.IsCapture = isCapture;
    }

    public Move(Move m)
    {
        var(src_row, src_col, dst_row, dst_col) = m;
        this.src_row = src_row;
        this.src_col = src_col;
        this.dest_row = dst_row;
        this.dest_col = dst_col;
        this.IsCapture = m.IsCapture;
    }

    public override bool Equals(object? obj)
    {
        if(obj == null || typeof(Move).IsInstanceOfType(obj))
            return false;

        Move other = (Move)obj;
        
        if(other.src_col != src_col)
            return false;

        if (other.src_row != src_col)
            return false;

        if (other.dest_col != dest_col)
            return false;

        if (other.dest_row!= dest_row)
            return false;

        if (other.IsCapture != IsCapture)
            return false;

        if (NextMove == null)
            return true;
        else 
            return NextMove.Equals(other.NextMove);

    }

    public override string ToString()
    {
        string result;
        if (NextMove is null)
            result = $"({src_row},{src_col}) -> ({dest_row},{dest_col})";
        else
            result = $"({src_row},{src_col}) -> {NextMove}";

        return result;
    }
}


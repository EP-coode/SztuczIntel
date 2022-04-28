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

    public Move(int src_x, int src_y, int dest_x, int dest_y, bool isCapture = false)
    {
        this.src_row = src_x;
        this.src_col = src_y;
        this.dest_row = dest_x;
        this.dest_col = dest_y;
        this.IsCapture = isCapture;
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


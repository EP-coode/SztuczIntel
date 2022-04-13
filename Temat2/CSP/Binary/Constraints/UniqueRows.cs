using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Binary.Constraints;

public class UniqueRow : Constraint<(int, int), int>
{
    private int boardHeight;
    private int boardWidth;

    public UniqueRow(ICollection<(int, int)> dependsOnVariables, int boardHeight, int boardWidth) : base(dependsOnVariables)
    {
        this.boardHeight = boardHeight;
        this.boardWidth = boardWidth;
    }

    public override bool Satisfied(Dictionary<(int, int), int> assigment, (int, int) changedVariable)
    {
        var (cx, cy) = changedVariable;
        int[] checkedRow = new int[boardWidth];

        for (int x = 0; x < boardWidth; x++)
        {
            checkedRow[x] = assigment[(x, cy)];
        }

        for (int y=0;y<boardHeight; y++)
        {
            bool rowsEqual = true;
            for(int x = 0; x < boardWidth; x++)
            {
                if (!assigment.ContainsKey((x, y)))
                {
                    rowsEqual = false;
                    break;
                }
                else if(assigment[(x,y)] != checkedRow[x])
                {
                    rowsEqual = false;
                    break;
                }
            }

            if (rowsEqual)
                return false;
        }

        return true;
    }
}


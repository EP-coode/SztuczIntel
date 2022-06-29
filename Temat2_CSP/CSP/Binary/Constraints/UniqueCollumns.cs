using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Binary.Constraints;

public class UniqueCollumns : Constraint<(int, int), int>
{
    private int boardHeight;
    private int boardWidth;

    public UniqueCollumns(ICollection<(int, int)> dependsOnVariables, int boardHeight, int boardWidth) : base(dependsOnVariables)
    {
        this.boardHeight = boardHeight;
        this.boardWidth = boardWidth;
    }

    public override bool Satisfied(Dictionary<(int, int), int> assigment, (int, int) changedVariable)
    {
        var (cx, cy) = changedVariable;
        int[] checkedCollumn = new int[boardHeight];


        for (int y = 0; y < boardHeight; y++)
        {
            if (!assigment.ContainsKey((cx, y)))
            {
                continue;
            }
            checkedCollumn[y] = assigment[(cx, y)];
        }

        for (int x = 0; x < boardWidth; x++)
        {
            if (x == cx)
            {
                continue;
            }

            bool colEqual = true;
            for (int y = 0; y < boardHeight; y++)
            {
                if (!assigment.ContainsKey((x, y)))
                {
                    colEqual = false;
                    break;
                }
                else if (assigment[(x, y)] != checkedCollumn[y])
                {
                    colEqual = false;
                    break;
                }
            }

            if (colEqual)
                return false;
        }

        return true;
    }
}


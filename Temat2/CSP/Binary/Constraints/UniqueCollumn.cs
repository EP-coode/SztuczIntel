using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Binary.Constraints;

public class UniqueCollumn : Constraint<(int,int), int>
{
    private int boardHeight;
    private int boardWidth;

    public UniqueCollumn(ICollection<(int, int)> dependsOnVariables, int boardHeight, int boardWidth) : base(dependsOnVariables)
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
            checkedCollumn[y] = assigment[(cx, y)];
        }

        for (int x = 0; x < boardWidth; x++)
        {
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP.Constraints;
public class NoRepeatsInRowsAndCols : IConstraint { 
    public bool IsValid(Variable[][] state, int x, int y)
    {
        var value = state[y][x].Value;


        for (int i = 0; i < state[y].Length; i++)
        {
            if (state[y][i].Value == -1)
                continue;
            if (state[y][i].Value == value && i != x)
                return false;
        }

        for (int i = 0; i < state.Length; i++)
        {
            if (state[i][x].Value == -1)
                continue;
            if (state[i][x].Value == value && i != y)
                return false;
        }

        return true;
    }
}


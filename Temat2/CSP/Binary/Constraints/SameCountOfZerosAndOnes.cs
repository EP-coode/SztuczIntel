using CSP.CSPBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Binary.Constraints;

public class SameCountOfZerosAndOnes : Constraint<(int, int), int>
{
    public SameCountOfZerosAndOnes(ICollection<(int, int)> dependsOnVariables) : base(dependsOnVariables)
    {
    }

    public override bool Satisfied(Dictionary<(int, int), int> assigment, (int, int) changedVariable)
    {
        int zeros = 0;
        int ones = 0;
        int empty = 0;

        foreach (var variable in DependentVariables)
        {
            if (!assigment.ContainsKey(variable))
            {
                empty++;
                continue;
            }

            switch (assigment[variable])
            {
                case 0:
                    zeros++;
                    break;
                case 1:
                    ones++;
                    break;
            }
        }

        if(Math.Abs(zeros - ones) > empty)
            return false;
        else
            return true;
        
    }
}


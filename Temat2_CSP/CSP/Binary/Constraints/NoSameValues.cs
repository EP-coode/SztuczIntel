using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Binary.Constraints;

internal class NoSameValues : Constraint<(int, int), int>
{
    private (int, int)[] dependOnVariablesArr;
    public NoSameValues(ICollection<(int, int)> dependsOnVariables) : base(dependsOnVariables)
    {
        if (dependsOnVariables.Count() < 0)
            throw new InvalidOperationException();

        dependOnVariablesArr = DependentVariables.ToArray();
    }

    public override bool Satisfied(Dictionary<(int, int), int> assigment, (int, int) changedVariable)
    {
        var first = dependOnVariablesArr[0];

        if (!assigment.ContainsKey(first))
        {
            return true;
        }

        for (int i = 1; i < dependOnVariablesArr.Length; i++)
        {
            if (!assigment.ContainsKey(dependOnVariablesArr[i]))
                return true;
            if (assigment[first] != assigment[dependOnVariablesArr[i]])
                return true;
        }

        return false;
    }
}


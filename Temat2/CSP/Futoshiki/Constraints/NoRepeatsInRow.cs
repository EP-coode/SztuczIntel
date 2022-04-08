using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Futoshiki.Constraints;

public class NoRepeatsInRow<V, D> : Constraint<V, D>
{
    public NoRepeatsInRow(ICollection<V> dependsOnVariables) : base(dependsOnVariables)
    {
    }

    public override bool Satisfied(Dictionary<V, D> assigment,V changedVariable)
    {
        List<D> alreadyPresentValues = new List<D>();   

        foreach(var variable in DependentVariables)
        {
            if (!assigment.ContainsKey(variable))
                continue;

            if (alreadyPresentValues.Contains(assigment[variable]))
                return false;

            alreadyPresentValues.Add(assigment[variable]);
        }

        return true;
    }
}


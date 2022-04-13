using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.CSPBase.VariableSelection;

public class BiggestAmmoutOfConstraints<V, D> : IVariableSelectionStrategy<V, D>
{
    public V? SelectVariable(Dictionary<V, D> assigments, ICollection<V> variables, Dictionary<V, List<Constraint<V, D>>> constraints)
    {
        List<V> emptyVariables = new();

        foreach(var v in variables)
        {
            if (!assigments.ContainsKey(v))
            {
                emptyVariables.Add(v);
            }
        }

        V bestVariable = emptyVariables[0];
        int bestVariableConstraintCount = constraints[bestVariable].Count();

        foreach(var v in emptyVariables)
        {
            int currentVariableConstraintCount = constraints[v].Count();
            if (currentVariableConstraintCount > bestVariableConstraintCount)
            {
                bestVariable = v;
                bestVariableConstraintCount = currentVariableConstraintCount;
            }
        }

        return bestVariable;

    }
}


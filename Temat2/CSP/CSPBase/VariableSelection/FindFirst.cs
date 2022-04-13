using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSPBase;

public class FindFirst<V, D> : IVariableSelectionStrategy<V, D>
{
    public V? SelectVariable(Dictionary<V, D> assigments, ICollection<V> variables,Dictionary<V, List<Constraint<V, D>>> constraints)
    {
        foreach (var variable in variables)
        {
            if (!assigments.ContainsKey(variable))
            {
                return variable;
            }
        }

        return default(V);
    }
}


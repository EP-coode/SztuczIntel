using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;

namespace CSP.Futoshiki.Constraints;

internal class GreaterThan<V> : Constraint<V, int>
{
    private V smaller;
    private V larger;

    public GreaterThan(V smaller, V larger) : base(new List<V> { smaller, larger})
    {
        this.smaller = smaller;
        this.larger = larger;
    }

    public override bool Satisfied(Dictionary<V, int> assigment, V changedVariable)
    {
        if(!assigment.ContainsKey(smaller) || !assigment.ContainsKey(larger))
        {
            return true;
        }

        return assigment[smaller] < assigment[larger];
    }
}


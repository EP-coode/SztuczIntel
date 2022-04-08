using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSPBase;

public abstract class Constraint<V, D>
{
    public ICollection<V> DependentVariables { get; set; }

    public Constraint(ICollection<V> dependsOnVariables)
    {
        DependentVariables = dependsOnVariables;
    }

    public abstract bool Satisfied(Dictionary<V, D> assigment, V changedVariable);

}


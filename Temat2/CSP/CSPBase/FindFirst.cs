using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSPBase;

public class FindFirst<V, D> : IVariableSelectionStrategy<V, D>
{
    public V SelectVariable(Dictionary<V, D> assigments, ICollection<V> variables)
    {
        throw new NotImplementedException();
    }
}


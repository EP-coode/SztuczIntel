using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP;

public interface FC
{
    public bool FC(Variable[][] state, ICollection<IConstraint> constraints,  int x, int y);
}


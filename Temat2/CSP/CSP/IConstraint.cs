using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP;
public interface IConstraint
{
    public bool IsValid(Variable[][] state, int x, int y);
}


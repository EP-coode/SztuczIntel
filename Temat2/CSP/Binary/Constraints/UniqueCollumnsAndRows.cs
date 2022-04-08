using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSP;

namespace CSP.Binary.Constraints;

public class UniqueCollumnsAndRows : Constraint<(int, int), int>
{
    public UniqueCollumnsAndRows(ICollection<(int, int)> dependsOnVariables) : base(dependsOnVariables)
    {
    }

    public override bool Satisfied(Dictionary<(int, int), int> assigment, (int, int) changedVariable)
    {
        throw new NotImplementedException();
        //var (x, y) = changedVariable;

        //List<List<int>> rows = new List<List<int>>();
        //List<List<int>> cols = new List<List<int>>();
    }
}


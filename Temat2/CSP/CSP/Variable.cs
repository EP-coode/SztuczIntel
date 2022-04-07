using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP;
public class Variable
{
    private int _value;

    public ICollection<int> Domain { get; set; }
    public int Value { get { return _value; } set { _value = value; } }

    public Variable(ICollection<int> domain, int value)
    {
        _value = value;
        Domain = domain;
    }

    public Variable(Variable variable)
    {
        Domain = new List<int>();
        foreach (var item in variable.Domain)
        {
            Domain.Add(item);
        }
        _value = variable.Value;
    }
}


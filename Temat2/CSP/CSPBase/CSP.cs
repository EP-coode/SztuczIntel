using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSPBase;

public class CSP<V, D>
{
    private ICollection<V> variables;
    private Dictionary<V, ICollection<D>> domains;
    private Dictionary<V, List<Constraint<V, D>>> constraints;
    private IVariableSelectionStrategy<V, D> selectionStrategy;

    public CSP(ICollection<V> variables,
        Dictionary<V, ICollection<D>> domains,
        IVariableSelectionStrategy<V, D> selectionStrategy)
    {
        this.variables = variables;
        this.domains = domains;
        this.selectionStrategy = selectionStrategy;
        constraints = new Dictionary<V, List<Constraint<V, D>>>();

        foreach (V variable in variables)
        {
            constraints.Add(variable, new List<Constraint<V, D>>());
            if (!domains.ContainsKey(variable))
            {
                throw new InvalidOperationException("Variable must have domain");
            }
        }
    }

    public ICollection<Dictionary<V, D>> BT(Dictionary<V, D> assigment)
    {
        ICollection<Dictionary<V, D>> results = new List<Dictionary<V, D>>();

        if (assigment.Count() == variables.Count())
        {
            results.Add(assigment);
            return results;
        }

        V unassigned = selectionStrategy.SelectVariable(assigment, variables);

        foreach(var value in domains[unassigned]) {

            Dictionary<V, D> localAssigment = new(assigment);
            localAssigment.Add(unassigned, value);

            if(IsConsistent(unassigned, localAssigment))
            {
                var res = BT(localAssigment);
                results = results.Concat(res).ToList();
            }
        }

        return results;
    }

    private bool IsConsistent(V assignedVariable, Dictionary<V, D> assigment)
    {
        foreach (var constraint in constraints[assignedVariable])
        {
            if (!constraint.Satisfied(assigment))
            {
                return false;
            }
        }

        return true;
    }

    public void AddConstraint(Constraint<V, D> constraint)
    {
        foreach (var v in constraint.DependentVariables)
        {
            if (!variables.Contains(v))
            {
                throw new InvalidOperationException("Constraint contains values not contained in CSP");
            }
            else
            {
                constraints[v].Add(constraint);
            }
        }
    }
}

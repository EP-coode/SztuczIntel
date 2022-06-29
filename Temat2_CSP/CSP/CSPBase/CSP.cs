using CSP.Futoshiki.Constraints;
using CSP.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    // only for measurements // very messy
    public int nodeCount = 0;
    public int firstSollution = 0;
    public int backTrackCount = 0;
    Stopwatch totalStopwatch = new Stopwatch();
    Stopwatch firststopwatch = new Stopwatch();

    private void StartMeasurements()
    {
        nodeCount = 0;
        firstSollution = 0;
        backTrackCount = 0;
        firststopwatch.Restart();
        totalStopwatch.Restart();
    }

    public (int, int, int, long, long) GetMeasurements()
    {
        return (nodeCount, firstSollution, backTrackCount, firststopwatch.ElapsedMilliseconds, totalStopwatch.ElapsedMilliseconds);
    }

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

    public ICollection<Dictionary<V, D>> _BT(Dictionary<V, D> assigment)
    {
        StartMeasurements();
        var r = BT(assigment);
        totalStopwatch.Stop();
        return r;
    }

    public ICollection<Dictionary<V, D>> BT(Dictionary<V, D> assigment)
    {
        nodeCount++;

        ICollection<Dictionary<V, D>> results = new List<Dictionary<V, D>>();

        if (assigment.Count() == variables.Count())
        {
            if (firstSollution == 0)
            {
                firstSollution = nodeCount;
                firststopwatch.Stop();
            }
            results.Add(assigment);
            return results;
        }

        V unassigned = selectionStrategy.SelectVariable(assigment, variables, constraints);

        foreach (var value in domains[unassigned])
        {

            Dictionary<V, D> localAssigment = new(assigment);
            localAssigment.Add(unassigned, value);

            if (IsConsistent(unassigned, localAssigment))
            {
                var res = BT(localAssigment);
                results = results.Concat(res).ToList();
            }
            else
            {
                backTrackCount++;
            }
        }
        return results;
    }

    public ICollection<Dictionary<V, D>> FC(Dictionary<V, D> assigment)
    {
        StartMeasurements();

        // check initial variables
        foreach (var variable in variables)
        {
            bool isConsistent = CutVariableDomains(variable, assigment, domains);
            if (!isConsistent)
            {
                return new List<Dictionary<V, D>>();
            }
        }

        var res = FC(assigment, domains);
        totalStopwatch.Stop();
        return res;
    }

    // returns false if some domain is empty
    private bool CutVariableDomains(V variableToCheck, Dictionary<V, D> assigment, Dictionary<V, ICollection<D>> domains)
    {
        bool consistentFlag = true;

        // for each constraint
        foreach (var constraint in constraints[variableToCheck])
        {
            // for each dependent value of constraint
            foreach (var variable in constraint.DependentVariables)
            {
                // skip self and assigned values
                if (variable.Equals(variableToCheck) || assigment.ContainsKey(variable))
                    continue;

                List<D> elementsToRemove = new List<D>();
                Dictionary<V, D> localAssigmentCheckCoppy = new(assigment);


                // for each variable value in domain
                foreach (var localValue in domains[variable])
                {
                    localAssigmentCheckCoppy[variable] = localValue;

                    // check if it is satisfied
                    if (!constraint.Satisfied(localAssigmentCheckCoppy, variable))
                    {
                        elementsToRemove.Add(localValue);
                    }
                }

                // occured empty domain
                if (elementsToRemove.Count() >= domains[variable].Count())
                {
                    consistentFlag = false;
                    break;
                }

                foreach (var element in elementsToRemove)
                    domains[variable].Remove(element);
            }

            if (!consistentFlag)
            {
                break;
            }
        }

        return consistentFlag;
    }

    private ICollection<Dictionary<V, D>> FC(Dictionary<V, D> assigment, Dictionary<V, ICollection<D>> domains)
    {
        nodeCount++;

        ICollection<Dictionary<V, D>> results = new List<Dictionary<V, D>>();

        if (assigment.Count() == variables.Count())
        {
            if (firstSollution == 0)
            {
                firstSollution = nodeCount;
                firststopwatch.Stop();
            }
            results.Add(assigment);
            return results;
        }

        V unassigned = selectionStrategy.SelectVariable(assigment, variables, constraints);

        foreach (var value in domains[unassigned])
        {

            // shallow coppy data
            Dictionary<V, D> localAssigment = new(assigment);
            Dictionary<V, ICollection<D>> localDomains = new Dictionary<V, ICollection<D>>();

            foreach (var (k, v) in domains)
            {
                ICollection<D> domainCoppy = new List<D>();

                foreach (D domainElement in domains[k])
                {
                    domainCoppy.Add(domainElement);
                }

                localDomains[k] = domainCoppy;
            }
            // shallow coppy data

            localAssigment.Add(unassigned, value);

            bool isConsistent = CutVariableDomains(unassigned, localAssigment, localDomains);

            if (isConsistent)
            {
                var res = FC(localAssigment, localDomains);
                results = results.Concat(res).ToList();
            }
            else
            {
                backTrackCount++;
            }
        }

        return results;
    }

    private bool IsConsistent(V assignedVariable, Dictionary<V, D> assigment)
    {
        foreach (var constraint in constraints[assignedVariable])
        {
            if (!constraint.Satisfied(assigment, assignedVariable))
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

using CSP.CSPBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.Binary.Constraints;

namespace CSP.Binary;

public static class BinaryLoader
{
    public static (CSP<(int, int), int>, Dictionary<(int, int), int>) LoadProblem(string fileUrl, IVariableSelectionStrategy<(int, int), int> selectionStrategy)
    {
        string[] data = File.ReadAllLines(fileUrl);

        List<(int, int)> variables = new List<(int, int)>();
        List<Constraint<(int, int), int>> constraints = new List<Constraint<(int, int), int>>();
        Dictionary<(int, int), ICollection<int>> domains = new Dictionary<(int, int), ICollection<int>>();
        Dictionary<(int, int), int> assigments = new Dictionary<(int, int), int>();
        int domainSize = data.Length / 2 + 1;

        // load variables, values and domains
        for (int y = 0; y < data.Length; y++)
        {
            for (int x = 0; x < data[y].Length; x++)
            {

                var variable = (x, y);
                variables.Add(variable);
                domains.Add(variable, Enumerable.Range(0, 2).ToList());

                if (data[y][x] != 'x')
                {
                    assigments.Add(variable, int.Parse(data[y][x].ToString()));
                }
            }
        }

        CSP<(int, int), int> csp = new(variables, domains, selectionStrategy);

        // load same count of zeroz and ones constraints // rows
        for (int y = 0; y < data.Length; y++)
        {
            List<(int, int)> dependentVariables = new();
            for (int x = 0; x < data[y].Length; x++)
            {
                dependentVariables.Add((x, y));
            }
            csp.AddConstraint(new SameCountOfZerosAndOnes(dependentVariables));
        }

        // load same count of zeroz and ones constraints // cols
        for (int x = 0; x < data.Length; x++)
        {
            List<(int, int)> dependentVariables = new();

            for (int y = 0; y < data.Length; y++)
            {
                dependentVariables.Add((x, y));
            }
            csp.AddConstraint(new SameCountOfZerosAndOnes(dependentVariables));
        }

        // load uqnique collumns and row constraint
        csp.AddConstraint(new UniqueRows(variables, data.Length, data.Length));
        csp.AddConstraint(new UniqueCollumns(variables, data.Length, data.Length));

        // add no same values constraint // rows
        for (int y = 0; y < data.Length; y++)
        {
            for (int x = 2; x < data[y].Length; x++)
            {
                List<(int, int)> dependentVariables = new()
                {
                    (x, y),
                    (x - 1, y),
                    (x - 2, y)
                };
                csp.AddConstraint(new NoSameValues(dependentVariables));
            }
        }

        // add no same values constraint // cols
        for (int x = 0; x < data.Length; x++)
        {
            for (int y = 2; y < data.Length; y++)
            {
                List<(int, int)> dependentVariables = new()
                {
                    (x, y),
                    (x, y - 1),
                    (x, y - 2)
                };
                csp.AddConstraint(new NoSameValues(dependentVariables));
            }
        }

        return (csp, assigments);
    }
}


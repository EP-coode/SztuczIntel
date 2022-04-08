using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.CSPBase;
using CSP.Futoshiki.Constraints;


namespace CSP.Futoshiki;
public static class FutoshikiLoader
{
    public static (CSP<(int,int), int>, Dictionary<(int, int), int>) LoadProblem(string fileUrl)
    {
        string[] data = File.ReadAllLines(fileUrl);

        List<(int,int)> variables = new List<(int,int)> ();
        List<Constraint<(int, int), int>> constraints = new List<Constraint<(int, int), int>> ();
        Dictionary<(int, int), ICollection<int>> domains = new Dictionary<(int, int), ICollection<int>>();
        Dictionary<(int, int), int > assigments = new Dictionary<(int, int), int> ();
        int domainSize = data.Length;

        // load variables, values and domains
        for(int y = 0; y < data.Length; y++)
        {
            for(int x=0;x< data[y].Length; y++)
            {
                // we have a variable
                if(y%2 == 0 || x%2 == 0)
                {
                    var variable = (x, y);
                    variables.Add(variable);
                    domains.Add(variable, Enumerable.Range(1, domainSize).ToList());

                    if(data[y][x] != 'x')
                    {
                        assigments.Add(variable, int.Parse(data[y][x].ToString()));
                    }
                }
            }
        }

        CSP<(int, int), int> csp = new(variables, domains, new FindFirst<(int, int), int>());

        // load no repeats in rows constraints
        for (int y = 0; y < data.Length; y++)
        {
            List<(int, int)> dependsOnVariables = new List<(int, int)>();
            for (int x = 0; x < data[y].Length; y++)
            {
                // we have a variable
                if (y % 2 == 0 || x % 2 == 0)
                {
                    dependsOnVariables.Add((x, y));
                };
            }

           csp.AddConstraint(new NoRepeatsInRow<(int, int), int>(dependsOnVariables));
        }

        // load no repeats in cols
        for (int x = 0; x < data.Length; x++)
        {
            List<(int, int)> dependsOnVariables = new List<(int, int)>();
            for (int y = 0; y < data[x].Length; y++)
            {
                // we have a variable
                if (y % 2 == 0 || x % 2 == 0)
                {
                    dependsOnVariables.Add((x, y));
                };
            }

            csp.AddConstraint(new NoRepeatsInRow<(int, int), int>(dependsOnVariables));
        }

        return (csp, assigments);
    }
}


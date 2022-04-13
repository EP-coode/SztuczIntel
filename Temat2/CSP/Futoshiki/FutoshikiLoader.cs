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
    public static (CSP<(int,int), int>, Dictionary<(int, int), int>) LoadProblem(string fileUrl, IVariableSelectionStrategy<(int,int),int> selectionStrategy)
    {
        string[] data = File.ReadAllLines(fileUrl);

        List<(int,int)> variables = new List<(int,int)> ();
        List<Constraint<(int, int), int>> constraints = new List<Constraint<(int, int), int>> ();
        Dictionary<(int, int), ICollection<int>> domains = new Dictionary<(int, int), ICollection<int>>();
        Dictionary<(int, int), int > assigments = new Dictionary<(int, int), int> ();
        int domainSize = data.Length/2 + 1;

        // load variables, values and domains
        for(int y = 0; y < data.Length; y++)
        {
            for(int x=0;x< data[y].Length; x++)
            {
                // we have a variable
                if(y%2 == 0 && x%2 == 0)
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

        CSP<(int, int), int> csp = new(variables, domains, selectionStrategy);

        // load no repeats in rows constraints
        for (int y = 0; y < data.Length; y++)
        {
            List<(int, int)> dependsOnVariables = new List<(int, int)>();
            for (int x = 0; x < data[y].Length; x++)
            {
                // we have a variable
                if (y % 2 == 0 && x % 2 == 0)
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
                if (y % 2 == 0 && x % 2 == 0)
                {
                    dependsOnVariables.Add((x, y));
                };
            }

            csp.AddConstraint(new NoRepeatsInRow<(int, int), int>(dependsOnVariables));
        }

        // load all constraints betwen variables
        for (int y = 0; y < data.Length; y++)
        {
            for (int x = 0; x < data[y].Length; x++)
            {
                // we have a constraint in row
                if ((y % 2 == 0 && x % 2 == 1) && data[y][x] != '-')
                {
                    var var1 = (x - 1, y);
                    var var2 = (x + 1, y);
                    if(data[y][x] == '>')
                    {
                        csp.AddConstraint(new GreaterThan<(int,int)>(var2, var1));
                    }
                    else if(data[y][x] == '<')
                    {
                        csp.AddConstraint(new GreaterThan<(int, int)>(var1, var2));
                    }
                }
                // we have a constraint in collumn
                else if (y % 2 == 1 && data[y][x] != '-')
                {
                    var var1 = (x*2, y - 1);
                    var var2 = (x*2, y + 1);
                    if (data[y][x] == '>')
                    {
                        csp.AddConstraint(new GreaterThan<(int, int)>(var2, var1));
                    }
                    else if (data[y][x] == '<')
                    {
                        csp.AddConstraint(new GreaterThan<(int, int)>(var1, var2));
                    }
                } ;
            }
        }

        return (csp, assigments);
    }
}


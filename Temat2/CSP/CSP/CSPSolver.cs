using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP;
public class CSPProblem
{
    private Variable[][] state;
    private ICollection<IConstraint> constraints;
    private FC fc;

    public CSPProblem(Variable[][] initialState, ICollection<IConstraint> constraints, FC fc)
    {
        state = initialState;
        this.constraints = constraints;
        this.fc = fc;
    }

    public CSPProblem(CSPProblem problem)
    {
        // coppy variables
        state = new Variable[problem.state.Length][];
        for (int i = 0; i < problem.state.Length; i++)
        {
            state[i] = new Variable[problem.state[i].Length];
            for (int j = 0; j < state[i].Length; j++)
            {
                state[i][j] = new Variable(problem.state[i][j]);
            }
        }

        constraints = problem.constraints;
        this.fc = problem.fc;
    }

    private (int, int, bool) NextEmpty()
    {
        for (int i = 0; i < state.Length; i++)
        {
            for (int j = 0; j < state[i].Length; j++)
            {
                if (state[i][j].Value == -1)
                {
                    return (i, j, true);
                }
            }
        }

        return (-1, -1, false);
    }

    private (int, int, bool) NextEmptySmallestDomain()
    {
        List<(int, int, bool)> candidates = new();

        for (int i = 0; i < state.Length; i++)
        {
            for (int j = 0; j < state[i].Length; j++)
            {
                if (state[i][j].Value == -1)
                {
                    candidates.Add((i, j, true));
                }
            }
        }

        (int, int, bool) bestCandidate = (-1,-1, false);

        foreach (var candidate in candidates)
        {
            if (!bestCandidate.Item3)
            {
                bestCandidate = candidate;
            }
            else
            {
                int currentCandidateCout = state[bestCandidate.Item1][bestCandidate.Item2].Domain.Count();
                int newCandidateCount = state[candidate.Item1][candidate.Item2].Domain.Count();
                if (currentCandidateCout > newCandidateCount)
                {
                    bestCandidate = candidate;
                }
            }
        }

        //if (candidates.Count() > 0)
        //    bestCandidate = candidates[0];


        return bestCandidate;
    }

    private bool isValid(int x, int y)
    {
        foreach (var constraint in constraints)
        {
            if (!constraint.IsValid(state, x, y))
                return false;
        }
        return true;
    }

    public override string ToString()
    {
        var arr = this.state;
        StringBuilder sb = new();
        sb.AppendLine();

        foreach (var b in arr)
        {
            foreach (var c in b)
            {
                if (c.Value == -1)
                    sb.Append('-');
                else
                    sb.Append(c.Value.ToString());
            }
            sb.AppendLine();
        }
        sb.AppendLine();


        return sb.ToString();
    }

    public ICollection<Variable[][]> Solve()
    {
        var (y, x, found) = NextEmptySmallestDomain();
        var sollutions = new List<Variable[][]>();

        if (!found)
        {
            sollutions.Add(state);
            return sollutions;
        }

        var currentVariable = state[y][x];

        foreach (var newValue in currentVariable.Domain)
        {
            CSPProblem child = new(this);

            //if (state[2][1].Value == 3 && state[2][2].Value == 2)
            //{
            //    Console.WriteLine("AAA");
            //    Console.WriteLine(this);
            //}


            child.state[y][x].Value = newValue;

            bool isValid = child.isValid(x, y);
            bool isVaidFC = fc.FC(child.state, constraints, x, y);

            if (isValid && isVaidFC)
            {
                sollutions = sollutions.Concat(child.Solve()).ToList();
            }
        }

        return sollutions;
    }
}


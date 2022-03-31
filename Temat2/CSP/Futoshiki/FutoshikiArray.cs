using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Futoshiki;

public class FutoshikiArray
{
    private char[][] state;
    private char[][] constraints;
    private char[] domain;

    public FutoshikiArray(char[][] state, char[][] constraints)
    {
        this.state = state;
        this.constraints = constraints;
        this.domain = Enumerable.Range(1, state.Length).Select(e => e.ToString()[0]).ToArray();
    }

    public FutoshikiArray(FutoshikiArray f)
    {
        state = new char[f.state.Length][];
        constraints = new char[f.constraints.Length][];
        domain = f.domain;

        for (int i = 0; i < f.constraints.Length; i++)
        {
            constraints[i] = new char[f.constraints[i].Length];
            Array.Copy(f.constraints[i], constraints[i], f.constraints[i].Length);
        }

        for (int i = 0; i < f.state.Length; i++)
        {
            state[i] = new char[f.state[i].Length];
            Array.Copy(f.state[i], state[i], f.state[i].Length);
        }
    }

    public IEnumerable<char[][]> Solve()
    {
        var (y, x, found) = NextEmpty();
        var sollutions = new List<char[][]>();

        if (!found)
        {
            sollutions.Add(state);
            return sollutions;
        }

        foreach (var newValue in domain)
        {
            FutoshikiArray child = new(this);

            child.state[y][x] = newValue;

            bool isValid = child.isValid(x, y);

            if (isValid)
            {
                sollutions = sollutions.Concat(child.Solve()).ToList();
            }
        }

        return sollutions;
    }

    private (int, int, bool) NextEmpty()
    {
        for (int i = 0; i < state.Length; i++)
        {
            for (int j = 0; j < state[i].Length; j++)
            {
                if (state[i][j] == 'x')
                {
                    return (i, j, true);
                }
            }
        }

        return (-1, -1, false);
    }

    private bool isValid(int x, int y)
    {
        if (ConstraintsSatisfied(x, y))
        {
            if (NoRepeatsInRowsAndCols(x, y))
            {
                return true;
            }
        }
        return false;
    }

    private bool NoRepeatsInRowsAndCols(int x, int y)
    {
        char value = state[y][x];


        for (int i = 0; i < state[y].Length; i++)
        {
            if (state[y][i] == 'x')
                continue;
            if (state[y][i] == value && i != x)
                return false;
        }

        for (int i = 0; i < state.Length; i++)
        {
            if (state[i][x] == 'x')
                continue;
            if (state[i][x] == value && i != y)
                return false;
        }

        return true;
    }

    private bool ConstraintValid(char constraintType, char value1, char value2)
    {
        if (value1 == 'x' || value2 == 'x')
            return true;

        if (constraintType == '-')
            return true;

        int v1 = int.Parse(value1.ToString());
        int v2 = int.Parse(value2.ToString());

        if (constraintType == '>' && v1 > v2)
            return true;

        if (constraintType == '<' && v1 < v2)
            return true;

        return false;
    }

    private bool ConstraintsSatisfied(int x, int y)
    {
        char constraint = '-';
        char value2 = 'x';
        // ograniczenie z góry
        if (y > 0)
        {
            constraint = constraints[y * 2 - 1][x];
            value2 = state[y - 1][x];

            if (!ConstraintValid(constraint, value2, state[y][x]))
                return false;
        }

        // ograniczenie z dolu
        if (y < state.Length - 1)
        {
            constraint = constraints[y * 2 + 1][x];
            value2 = state[y + 1][x];

            if (!ConstraintValid(constraint, value2, state[y][x]))
                return false;
        }

        // ograniczenie z prawa
        if (x < state[y].Length - 1)
        {
            constraint = constraints[y * 2][x];
            value2 = state[y][x + 1];

            if (!ConstraintValid(constraint, value2, state[y][x]))
                return false;
        }

        // ograniczenie z lewa
        if (x > 0)
        {
            constraint = constraints[y * 2][x - 1];
            value2 = state[y][x - 1];

            if (!ConstraintValid(constraint, value2, state[y][x]))
                return false;
        }

        return true;
    }
}


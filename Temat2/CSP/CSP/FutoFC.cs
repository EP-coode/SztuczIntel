using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP;

public class FutoFC : FC
{
    private char[][] constraints;
    public FutoFC(char[][] constraints){ 
        this.constraints = constraints;
    }
    private bool EsureRowAndColsUnique(Variable[][] state, int x, int y)
    {
        int value = state[y][x].Value;

        for (int x1 = 0; x1 < state[y].Length; x1++)
        {
            if (state[y][x1].Value == -1)
            {
                state[y][x1].Domain.Remove(value);
                if (state[y][x1].Domain.Count() == 0)
                {
                    return false;
                }
            }
        }

        for (int y2 = 0; y2 < state.Length; y2++)
        {
            if (state[y2][x].Value == -1)
            {
                state[y2][x].Domain.Remove(value);
                if (state[y2][x].Domain.Count() == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ConstraintValid(char constraintType, int value1, int value2)
    {
        if (value1 == -1 || value2 == -1)
            return true;

        if (constraintType == '-')
            return true;

        if (constraintType == '>' && value1 > value2)
            return true;

        if (constraintType == '<' && value1 < value2)
            return true;

        return false;
    }

    private bool VariableValid(char constraintType, Variable value1, Variable value2)
    {
        if (value2.Value == -1)
        {
            ICollection<int> domain = value2.Domain;

            List<int> toRemove = new();

            foreach (var possibleValue in domain)
            {
                if (!ConstraintValid(constraintType, value1.Value, possibleValue))
                    toRemove.Add(possibleValue);
            }

            foreach (var value in toRemove)
            {
                domain.Remove(value);
            }

            if (domain.Count() == 0)
                return false;

        }

        return true;
    }


    private bool EnsureConstraintsVaild(Variable[][] state, int x, int y)
    {
        char constraint = '-';
        Variable value2 = null;
        Variable value1 = state[y][x];

        if (y > 0)
        {
            value2 = state[y - 1][x];
            constraint = constraints[y * 2 - 1][x];

            if (constraint == '<')
            {
                constraint = '>';
            }
            else if (constraint == '>')
            {
                constraint = '<';
            }

            if (!VariableValid(constraint, value1, value2))
                return false;
           
        }

        // ograniczenie z dolu
        if (y < state.Length - 1)
        {
            constraint = constraints[y * 2 + 1][x];
            value2 = state[y + 1][x];

            if (!VariableValid(constraint, value1, value2))
                return false;
        }

        // ograniczenie z prawa
        if (x < state[y].Length - 1)
        {
            constraint = constraints[y * 2][x];
            value2 = state[y][x + 1];

            if (!VariableValid(constraint, value1, value2))
                return false;
        }

        // ograniczenie z lewa
        if (x > 0)
        {
            constraint = constraints[y * 2][x - 1];

            if (constraint == '<')
            {
                constraint = '>';
            }
            else if (constraint == '>')
            {
                constraint = '<';
            }

            value2 = state[y][x - 1];

            if (!VariableValid(constraint, value1, value2))
                return false;
        }

        return true;
    }

    public bool FC(Variable[][] state, ICollection<IConstraint> constraints, int x, int y)
    {
        if (!EsureRowAndColsUnique(state, x, y))
            return false;

        if (!EnsureConstraintsVaild(state, x, y))
            return false;

        return true;
    }
}


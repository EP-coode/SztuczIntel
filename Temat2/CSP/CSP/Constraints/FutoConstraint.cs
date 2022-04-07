using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.CSP.Constraints;

public class FutoConstraint : IConstraint
{
    private char[][] constraints;

    public FutoConstraint(char[][] constraints)
    {
        this.constraints = constraints;
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


    public bool IsValid(Variable[][] state, int x, int y)
    {
        char constraint = '-';
        int value2 = -1;
        // ograniczenie z góry
        if (y > 0)
        {
            constraint = constraints[y * 2 - 1][x];
            if (constraint == '<')
            {
                constraint = '>';
            }
            else if (constraint == '>')
            {
                constraint = '<';
            }
            value2 = state[y - 1][x].Value;

            if (!ConstraintValid(constraint, state[y][x].Value, value2))
                return false;
        }

        // ograniczenie z dolu
        if (y < state.Length - 1)
        {
            constraint = constraints[y * 2 + 1][x];
            value2 = state[y + 1][x].Value;

            if (!ConstraintValid(constraint, state[y][x].Value, value2))
                return false;
        }

        // ograniczenie z prawa
        if (x < state[y].Length - 1)
        {
            constraint = constraints[y * 2][x];
            value2 = state[y][x + 1].Value;

            if (!ConstraintValid(constraint, state[y][x].Value, value2))
                return false;
        }

        // ograniczenie z lewa
        if (x > 0)
        {
            constraint = constraints[y * 2][x - 1];
            value2 = state[y][x - 1].Value;


            if (constraint == '<')
            {
                constraint = '>';
            }
            else if (constraint == '>')
            {
                constraint = '<';
            }

            if (!ConstraintValid(constraint, state[y][x].Value, value2))
                return false;
        }

        return true;
    }
}


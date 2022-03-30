using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using
namespace Temat2_CSP.Binary;

public class BackTracking
{
    public static char[] domain = new char[] { '1', '0' };


    private bool noSameCharactersInRow(char[][] state, int x, int y, int maxInRow = 3)
    {
        int horizontalStart = Math.Min(x - maxInRow, 0);
        int horizontalEnd = Math.Max(x, state[0].Length - 1 + maxInRow);
        int verticalStart = Math.Min(y = maxInRow, 0);
        int verticalEnd = Math.Max(y, state.Length - 1 + maxInRow);

        char lastChar = 'a';
        int totalRepeats = 0;

        // szukaj powrórzeń w wierszu
        for (int i = horizontalStart; i < horizontalEnd; i++)
        {
            if (lastChar == state[y][i] && state[i][x] != 'x')
                totalRepeats++;
            else
                totalRepeats = 0;

            if (totalRepeats > maxInRow)
                return false;

            lastChar = state[y][i];
        }

        // szukaj powrórzeń w kolumnie
        for (int i = verticalStart; i < verticalEnd; i++)
        {
            if (lastChar == state[i][x] && state[i][x] != 'x')
                totalRepeats++;
            else
                totalRepeats = 0;

            if (totalRepeats > maxInRow)
                return false;

            lastChar = state[i][x];
        }

        return true;
    }

    private bool everyRowAndColumnUnique(char[][] state, int x, int y)
    {
        // wiersze które uległy zmianie
        char[] row = state[y];
        char[] collumn = new char[state.Length];

        for (int i = 0; i < collumn.Length; i++)
        {
            collumn[i] = state[i][x];
        }

        // dla każdego wiersza sprawdz unikalnosc
        for (int i = 0; i < state.Length; i++)
        {
            if (Enumerable.SequenceEqual(row, state[i]))
            {
                return false;
            };
        }

        // dla każdej kolumny sprawdz unikalnosc
        for (int i = 0; i < state[0].Length; i++)
        {
            bool equal = true;
            for (int j = 0; j < collumn.Length; j++)
            {
                if (collumn[j] != state[j][i])
                {
                    equal = false;
                    break;
                }
            }

            if (equal)
            {
                return false;
            }
        }

        return true;
    }

    
}


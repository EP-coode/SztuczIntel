using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Temat2_CSP.Binary;

public class BackTracking
{
    public static char[] domain = new char[] { '0', '1' };
    public char[][] State { get; set; }

    public BackTracking(char[][] initialState)
    {
        State = initialState;
    }

    public BackTracking(BackTracking parent)
    {
        this.State = new char[parent.State.Length][];

        for (int i = 0; i < this.State.Length; i++)
        {
            this.State[i] = new char[parent.State[i].Length];
            for (int j = 0; j < parent.State[i].Length; j++)
            {
                this.State[i][j] = parent.State[i][j];
            }
        }
    }

    public ICollection<BackTracking> Solve()
    {
        var (x, y, found) = NextEmptySpot();
        ICollection<BackTracking> sollutions = new List<BackTracking>();

        if (!found)
        {
            sollutions.Add(this);
            return sollutions;
        }

        foreach (char domainItem in domain)
        {
            BackTracking child = new(this);
            child.State[y][x] = domainItem;
            if (child.isValid(x, y))
            {
                var childrenSollutions = child.Solve();
                sollutions = sollutions.Concat(childrenSollutions).ToList();
            }
        }

        return sollutions;
    }

    private (int, int, bool) NextEmptySpot()
    {
        for (int i = 0; i < State.Length; i++)
        {
            for (int j = 0; j < State[i].Length; j++)
            {
                if (State[i][j] == 'x')
                {
                    return (j, i, true);
                }
            }
        }

        return (-1, -1, false);
    }

    private bool isValid(int x, int y)
    {
        if (!AreCountOfCharactersValid(x, y))
            return false;

        if (!AreThereSameCharatersInARow(x, y))
            return false;

        if (!AreRowAndCollumnsUnique(x, y))
            return false;

        return true;
    }

    private bool AreThereSameCharatersInARow(int x, int y, int maxInRow = 2)
    {
        int horizontalStart = Math.Max(x - maxInRow, 0);
        int horizontalEnd = Math.Min(x + maxInRow, State[0].Length - 1);
        int verticalStart = Math.Max(y - maxInRow, 0);
        int verticalEnd = Math.Min(y + maxInRow, State.Length - 1);

        char lastChar = 'a';
        int totalRepeats = 1;

        // szukaj powrórzeń w wierszu
        for (int i = horizontalStart; i <= horizontalEnd; i++)
        {
            if (lastChar == State[y][i] && State[y][i] != 'x')
                totalRepeats++;
            else
                totalRepeats = 1;

            if (totalRepeats > maxInRow)
                return false;

            lastChar = State[y][i];
        }

        lastChar = 'a';
        totalRepeats = 1;
        // szukaj powrórzeń w kolumnie
        for (int i = verticalStart; i <= verticalEnd; i++)
        {
            if (lastChar == State[i][x] && State[i][x] != 'x')
                totalRepeats++;
            else
                totalRepeats = 1;

            if (totalRepeats > maxInRow)
                return false;

            lastChar = State[i][x];
        }

        return true;
    }

    private bool AreRowAndCollumnsUnique(int x, int y)
    {
        // wiersze które uległy zmianie
        char[] row = State[y];
        char[] collumn = new char[State.Length];

        for (int i = 0; i < collumn.Length; i++)
        {
            collumn[i] = State[i][x];
        }

        // dla każdego wiersza sprawdz unikalnosc
        for (int i = 0; i < State.Length; i++)
        {
            if (i == y)
                continue;

            bool equal = true;
            for (int j = 0; j < State[0].Length; j++)
            {
                if (row[j] != State[i][j] || row[j] == 'x' || State[i][j] == 'x')
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

        // dla każdej kolumny sprawdz unikalnosc
        for (int i = 0; i < State[0].Length; i++)
        {
            if (i == x)
                continue;

            bool equal = true;
            for (int j = 0; j < collumn.Length; j++)
            {
                if (collumn[j] != State[j][i] || collumn[j] == 'x' || State[j][i] == 'x')
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

    private bool AreCountOfCharactersValid(int x, int y)
    {
        char[] row = State[y];
        char[] collumn = new char[State.Length];

        for (int i = 0; i < collumn.Length; i++)
        {
            collumn[i] = State[i][x];
        }

        return isRowValid(row) && isRowValid(collumn);
    }

    private bool isRowValid(char[] row)
    {
        int zeros = 0;
        int ones = 0;
        int undefined = 0;

        for (int i = 0; i < row.Length; i++)
        {
            switch (row[i])
            {
                case '0':
                    zeros++;
                    break;
                case '1':
                    ones++;
                    break;
                default:
                    undefined++;
                    break;
            }

        }
        if (Math.Abs(zeros - ones) <= undefined)
        {
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        var s = new StringBuilder();

        for (var i = 0; i < State.Length; i++)
        {
            for (var j = 0; j < State[i].Length; j++)
            {
                s.Append(State[i][j]);
            }

            s.AppendLine();
        }

        return s.ToString();
    }


}


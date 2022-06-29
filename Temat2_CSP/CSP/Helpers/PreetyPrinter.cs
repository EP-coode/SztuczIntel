using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Helpers;

public static class PreetyPrinter
{
    public static string variablesToTable<D>(Dictionary<(int,int), D> assigments)
    {
        double sqr = Math.Sqrt(assigments.Count());

        if (sqr % 1 != 0)
            throw new InvalidOperationException();

        int size = (int)Math.Floor(sqr) * 2;

        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < size; y++)
        {

            for(int x = 0;x< size; x++)
            {
                if (assigments.ContainsKey((x, y)))
                {
                   sb.Append(assigments[(x,y)].ToString());
                }
                else
                {
                    sb.Append('-');
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temat2_CSP.Futoshiki;

public class FutoshikiUtils
{
    public static FutoshikiGraph LoadFromFile(string filePath)
    {
        string[] data = File.ReadAllLines(filePath);
        int boardSize = data.Length / 2 + 1;

        int[] domain = Enumerable.Range(1, boardSize).ToArray();

        FutoshikiGraph graph = new(domain);

        // load nodes
        for (int i = 0; i < data.Length; i += 2)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                int value;
                bool succes = int.TryParse(data[i][j].ToString(), out value);

                if (succes || data[i][j] == 'x')
                {
                    Node n = new()
                    {
                        Value = data[i][j] == 'x' ? -1 : value,
                        X = j,
                        Y = i,
                    };

                    graph.AddNode(n);
                }
            }
        }

        //load edges
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = i % 2 == 0 ? 1 : 0;
                j < data[i].Length;
                j += 2)
            {
                Node src;
                Node dst;
                if (i % 2 == 0)
                {
                    src = graph.GetNodeByPosition(j - 1, i);
                    dst = graph.GetNodeByPosition(j + 1, i);
                }
                else
                {
                    src = graph.GetNodeByPosition(j, i - 1);
                    dst = graph.GetNodeByPosition(j, i + 1);
                }
                switch (data[i][j])
                {
                    case '>':
                        graph.AddEdge(src, dst, Relation.GREATER_THAN);
                        break;
                    case '<':
                        graph.AddEdge(src, dst, Relation.LESS_THAN);
                        break;
                    default:
                        break;
                }
            }
        }

        return graph;
    }

    public static (char[][], char[][]) LoadFromFileV2(string filePath)
    {
        string[] data = File.ReadAllLines(filePath);

        char[][] variables = new char[data.Length / 2 + 1][];
        char[][] constraints = new char[data.Length][];

        for (int i = 0; i < data.Length; i++)
        {
            if (i % 2 == 0) {
                variables[i / 2] = new char[data[i].Length / 2 + 1];
                constraints[i] = new char[data[i].Length / 2];
            }
            else
            {
                constraints[i] = new char[data[i].Length];
            }


            for (int j = 0; j < data[i].Length; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    variables[i / 2][j / 2] = data[i][j];
                }
                else if(i % 2 == 0)
                {
                    constraints[i][j / 2] = data[i][j];
                }
                else if(i % 2 == 1)
                {
                    constraints[i][j] = data[i][j];
                }
            }
        }

        return (variables, constraints);
    }
}


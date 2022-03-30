using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temat2_CSP.Binary;

public class Utils
{
    public static char[][] LoadFromFile(string filePath)
    {
        string[] data = File.ReadAllLines(filePath);
        int boardHeight = data.Length;
        int boardWidth = data[0].Length;

        char[][] board = new char[boardHeight][];
        
        for(int i=0; i < boardHeight; i++)
        {
            board[i] = new char[boardWidth];
            for(int j=0; j < boardWidth; j++)
            {
                board[i][j] = data[i][j];
            }
        }

        return board;
    }
}


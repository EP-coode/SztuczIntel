using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChekersGame.Chekers;

namespace ChekersGame;

public class ConsolePlayer : Player
{
    public ConsolePlayer(PieceColor color) : base(color)
    {

    }

    public override Move MakeMove(Board b)
    {
        if(PlayerPieceColor == PieceColor.BLACK)
            Console.WriteLine($"----------GRACZ CZARNY------------");
        else
            Console.WriteLine($"----------GRACZ BIAŁY------------");

        int row, col;
        List<Move> moves = new List<Move>();
        do
        {
            Console.WriteLine("wybierz pionek: ");
            string input = Console.ReadLine();
            if (input is not null)
            {
                bool rowOk = int.TryParse(input[0].ToString(), out row);
                bool colOk = int.TryParse(input[1].ToString(), out col);
                if (rowOk && colOk && b[row, col] != null && b[row, col].PieceColor == PlayerPieceColor)
                {
                    moves = b.GetAllPossibleMoves(b[row, col]);
                    if (moves.Count() > 0)
                        break;
                }
            }
            Console.WriteLine("wybrałeś nieodpowiedni pionek.");
        }
        while (true);

        Console.WriteLine("Dostępne róchy: ");
        for (int i = 0; i < moves.Count(); i++)
        {
            Console.WriteLine($"{i}. {moves[i]}");
        }

        int idRochu;

        do
        {
            Console.WriteLine("Wybierz róch: ");
            string input = Console.ReadLine();
            bool ok = int.TryParse(input, out idRochu);
            if (ok && idRochu < moves.Count() && idRochu >= 0)
            {
                break;
            }
            Console.WriteLine("Możesz wybierać tylko z pośród podanych ruchów!");
        }
        while (true);

        return moves[idRochu];
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers.Players;

public class RandomPlayer : Player
{
    public static Random rnd = new Random();

    public RandomPlayer(PieceColor color) : base(color)
    {
    }

    public override Move MakeMove(Board b)
    {
        var moves = b.GetAllPossibleMoves(PlayerPieceColor);
        int index = rnd.Next(moves.Count);
        Console.WriteLine(moves[index]);
        return moves[index];    
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public abstract class Player
{
    public PieceColor PlayerPieceColor { get; }

    public Player(PieceColor color)
    {
        PlayerPieceColor = color;
    }

    public abstract Move MakeMove(Board b);
}


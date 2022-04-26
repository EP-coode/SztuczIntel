using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public enum Player
{
    WHITE,
    BLACK,
}

public class Game
{
    public Player CurrentPlayer { get; set; }
    public Board GameBoard { get; set; }

    public Game()
    {
        CurrentPlayer = Player.WHITE;
        GameBoard = new Board();
    }

    /// <summary>
    /// Determins if the game is Finished
    /// </summary>
    /// <returns>
    /// true if game is finished
    /// </returns>
    public bool IsFinished()
    {
        throw new NotImplementedException();
    }


}

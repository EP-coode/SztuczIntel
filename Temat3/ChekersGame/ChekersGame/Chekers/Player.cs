using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers;

public interface IPlayer
{
    public Move MakeMove(Board b);
}


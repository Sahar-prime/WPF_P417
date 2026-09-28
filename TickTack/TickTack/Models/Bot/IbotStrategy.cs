using System;
using System.Collections.Generic;
using System.Text;

namespace TickTack.Models.Bot
{
    public interface IbotStrategy
    {
        (int Row, int Col) ChooseMove(GameBoard board, Player bot, Player opponent);
    }
}

using Chess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Bussiness
{
    public class CheckSolution
    {
        public static bool CheckMove(string move, int moveNumber, List<Move> moves)
        {
            if(moves == null || moves.Count == 0 || moveNumber >= moves.Count)
                return false;
            if (moves[moveNumber].Content.Equals(move))
                return true;
            return false;
        }
    }
}

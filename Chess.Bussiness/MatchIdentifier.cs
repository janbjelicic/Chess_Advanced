using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Bussiness
{
    public class MatchIdentifier
    {
        public static string CalculateMatchIdentifier(int whiteId, int blackId, int time, int gain)
        {
            return string.Format("{0}{1}{2}{3}", whiteId.ToString().GetHashCode(), 
                blackId.ToString().GetHashCode(), time.ToString().GetHashCode(), gain.ToString().GetHashCode());
        }

        public static bool CheckMatchIdentifier(int whiteId, int blackId, int time, int gain, string identifier)
        {
            return identifier.Equals(CalculateMatchIdentifier(whiteId, blackId, time, gain));
        }
    }
}

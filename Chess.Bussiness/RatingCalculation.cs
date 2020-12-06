using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Bussiness
{
    /// <summary>
    /// Class that will be used for calculating elo and problem rating.
    /// </summary>
    public class RatingCalculation
    {
        /// <summary>
        /// Calculating elo with an official formula.
        /// dE = K * (S - 1 / (10 ^ ((-D/F) + 1)))
        /// dE : pending Elo points
        /// S : Score (1 = won, 0 = lost, 0.5 = draw)
        /// F : F factor (rating disparity) = 400 : the higher is F, the easier it is to gain points (or to lose them)
        /// D : Score difference : Player Elo score - Opponent Elo score
        /// K : K Factor (game importance), computed as stated below :
        ///     The score of one of the players is less than 2100, K will be 32
        ///     The score of one of the players is less than 2401, K will be 24
        ///     Both players have a score > 2400, K will be 16
        /// </summary>
        /// <param name="playerRating">Rating of the player for which we calculate rating.</param>
        /// <param name="opponentRating">Rating of the opponent.</param>
        /// <param name="score">1 if white won, 0 if black won and 0.5 if there was a draw.</param>
        /// <returns>New elo rating.</returns>
        public static double CalculateElo(int playerRating, int opponentRating, double score)
        {
            int k = 16;
            if (playerRating < 2100 || opponentRating < 2100)
                k = 32;
            else if (playerRating < 2401 || opponentRating < 2401)
                k = 24;
            double d = playerRating - opponentRating;
            double f = 400;
            double final = k * (score - 1 / Math.Pow(10.0, ((-d / f) + 1)));
            if (final < 0)
                return 0;
            if (final > 3000)
                return 3000;
            return final;
        }
        /// <summary>
        /// Calculates problem rating.
        /// </summary>
        /// <param name="currentRating">Current problem rating of the player.</param>
        /// <param name="difficulty">Difficulty of the problem.</param>
        /// <returns>New problem rating.</returns>
        public static int CalculateRatingProblem(int currentRating, int difficulty)
        {
            return currentRating + difficulty - 1;
        }
    }
}

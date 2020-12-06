using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Domain.Entities;
namespace Chess.Bussiness.Services.Matches
{
    public class MatchService
    {
        /// <summary>
        /// Creating the description of the match from the following parameters.
        /// </summary>
        /// <param name="whiteName">Name of the white player.</param>
        /// <param name="blackName">Name of the black player.</param>
        /// <param name="result">Match result.</param>
        /// <param name="place">Place where the match was played.</param>
        /// <param name="date">Date when the match was played.</param>
        /// <returns>Match description.</returns>
        public static string GetDescription(string whiteName, string blackName, string result, string place, string date)
        {
            return string.Format("{0} - {1} {2}, {3} {4}", whiteName, blackName, result, place, date);
        }        
    }
}

using System.Linq;
using Chess.Domain.Entities;

namespace Chess.Domain.Abstract
{
    public interface IMatchRepository
    {
        /// <summary>
        /// Return all matches from the database.
        /// </summary>
        IQueryable<Match> Matches { get; }
        /// <summary>
        /// Saves the match to database.
        /// </summary>
        /// <param name="match">Match that will be inserted in the database.</param>
        void SaveMatch(Match match);
        /// <summary>
        /// Updates match information in the database.
        /// </summary>
        /// <param name="match">Match which information will be updated in the database.</param>
        void UpdateMatch(Match match);
        /// <summary>
        /// Deleting match from database.
        /// </summary>
        /// <param name="match">Match that will be deleted from the database.</param>
        void DeleteMatch(Match match);
        /// <summary>
        /// Gets single match.
        /// </summary>
        /// <param name="id">Identifier with which we determine which match has to be returned.</param>
        /// <returns>Match specified by the identifier.</returns>
        Match FindMatchById(int id);
    }
}
using Chess.Domain.Entities;
using System.Linq;

namespace Chess.Domain.Abstract
{
    public interface IGameRepository
    {
        /// <summary>
        /// Return all games from the database.
        /// </summary>
        IQueryable<Game> Games { get; }
        /// <summary>
        /// Saves the game to database.
        /// </summary>
        /// <param name="game">Game that will be inserted in the database.</param>
        void SaveGame(Game game);
        /// <summary>
        /// Updates game information in the database.
        /// </summary>
        /// <param name="game">Game which information will be updated in the database.</param>
        void UpdateGame(Game game);
        /// <summary>
        /// Deleting game from database.
        /// </summary>
        /// <param name="game">Game that will be deleted from the database.</param>
        void DeleteGame(Game game);
        /// <summary>
        /// Gets single game.
        /// </summary>
        /// <param name="id">Identifier with which we determine which game has to be returned.</param>
        /// <returns>Game specified by the identifier.</returns>
        Game FindGameById(int id);
    }
}

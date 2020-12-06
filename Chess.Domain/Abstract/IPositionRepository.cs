using System.Linq;
using Chess.Domain.Entities;

namespace Chess.Domain.Abstract
{
    public interface IPositionRepository
    {
        /// <summary>
        /// Return all positions from the database.
        /// </summary>
        IQueryable<Position> Positions { get; }
        /// <summary>
        /// Saves the position to database.
        /// </summary>
        /// <param name="position">Position that will be inserted in the database.</param>
        void SavePosition(Position position);
        /// <summary>
        /// Updates position information in the database.
        /// </summary>
        /// <param name="position">Position which information will be updated in the database.</param>
        void UpdatePosition(Position position);
        /// <summary>
        /// Deleting position from database.
        /// </summary>
        /// <param name="position">Position that will be deleted from the database.</param>
        void DeletePosition(Position position);
        /// <summary>
        /// Gets single position.
        /// </summary>
        /// <param name="id">Identifier with which we determine which position has to be returned.</param>
        /// <returns>Position specified by the identifier.</returns>
        Position FindPositionById(int id);
    }
}
using Chess.Domain.Entities;
using System.Linq;

namespace Chess.Domain.Abstract
{
    public interface IUserSolutionRepository
    {
        /// <summary>
        /// Return all user solutions from the database.
        /// </summary>
        IQueryable<UserSolution> UserSolutions { get; }
        /// <summary>
        /// Saves the user solution to database.
        /// </summary>
        /// <param name="userSolution">User solution that will be inserted in the database.</param>
        void SaveUserSolution(UserSolution userSolution);
        /// <summary>
        /// Updates user solution information in the database.
        /// </summary>
        /// <param name="userSolution">User solution which information will be updated in the database.</param>
        void UpdateUserSolution(UserSolution userSolution);
        /// <summary>
        /// Deleting user solution from database.
        /// </summary>
        /// <param name="userSolution">User solution that will be deleted from the database.</param>
        void DeleteUserSolution(UserSolution userSolution);
        /// <summary>
        /// Gets single user solution.
        /// </summary>
        /// <param name="id">Identifier with which we determine which user solution has to be returned.</param>
        /// <returns>User solution specified by the identifier.</returns>
        UserSolution FindUserSolutionById(int id);
        /// <summary>
        /// Gets single user solution.
        /// </summary>
        /// <param name="userId">Identifier with which we determine user of the user solution that has to be returned.</param>
        /// <param name="positionId">Identifier with which we determine position of the user solution that has to be returned.</param>
        /// <returns>User solution specified by the identifiers.</returns>
        UserSolution FindUserSolutionByIdentifiers(int userId, int positionId);
    }
}

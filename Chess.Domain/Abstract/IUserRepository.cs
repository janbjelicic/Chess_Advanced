using System.Collections.Generic;
using System.Linq;
using Chess.Domain.Entities;

namespace Chess.Domain.Abstract
{
    public interface IUserRepository
    {
        /// <summary>
        /// Return all users from the database.
        /// </summary>
        IQueryable<User> Users { get; }
        /// <summary>
        /// Saves the user to database.
        /// </summary>
        /// <param name="user">User that will be inserted in the database.</param>
        void SaveUser(User user);
        /// <summary>
        /// Updates user information in the database.
        /// </summary>
        /// <param name="user">User which information will be updated in the database.</param>
        void UpdateUser(User user);
        /// <summary>
        /// Deleting user from database.
        /// </summary>
        /// <param name="user">User that will be deleted from the database.</param>
        void DeleteUser(User user);
        /// <summary>
        /// Gets single user.
        /// </summary>
        /// <param name="id">Identifier with which we determine which user has to be returned.</param>
        /// <returns>User specified by the identifier.</returns>
        User FindUserById(int id);
        /// <summary>
        /// Gets single user.
        /// </summary>
        /// <param name="userName">User name with which we determine which user has to be returned.</param>
        /// <returns>User specified by the user name.</returns>
        User FindUserByUserName(string userName);
        /// <summary>
        /// Gets user comments.
        /// </summary>
        /// <param name="id">Identifier with which we determine comments from which user should be returned.</param>
        /// <returns>List of user comments.</returns>
        List<Comment> GetUserComments(int id);
        /// <summary>
        /// Gets user matches.
        /// </summary>
        /// <param name="id">Identifier with which we determine matches from which user should be returned.</param>
        /// <returns>List of user matches.</returns>
        List<Match> GetUserMatches(int id);
        /// <summary>
        /// Adding the user comment.
        /// </summary>
        /// <param name="comment">Comment that will be inserted in the database.</param>
        void AddUserComment(Comment comment);
        /// <summary>
        /// Deletes the user comment.
        /// </summary>
        /// <param name="comment">Comment that will be deleted.</param>
        void DeleteUserComment(Comment comment);
    }
}
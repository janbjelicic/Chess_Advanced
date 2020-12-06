using Chess.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Bussiness.Services.Users
{
    public class UserService
    {
        /// <summary>
        /// Retrieves the best players from the list ordered
        /// by the playing rating in a Player Rating format.
        /// </summary>
        /// <param name="users">Players on the site.</param>
        /// <param name="count">Number of players that will be taken as the best.</param>
        /// <returns>List of results.</returns>
        public static List<string> GetBestPlayers(List<User> users, int count)
        {
            return users.OrderByDescending(x => x.PlayingRating).Take(count).Select(x => string.Format("{0} {1}", x.UserName, x.PlayingRating)).ToList();
        }

        /// <summary>
        /// Retrieves the best problem solvers from the list ordered
        /// by the playing rating in a Player Rating format.
        /// </summary>
        /// <param name="users">Players on the site.</param>
        /// <param name="count">Number of problem solvers that will be taken as the best.</param>
        /// <returns>List of results.</returns>
        public static List<string> GetBestProblemSolvers(List<User> users, int count)
        {
            return users.OrderByDescending(x => x.ProblemRating).Take(count).Select(x => string.Format("{0} {1}", x.UserName, x.ProblemRating)).ToList();
        }
    }
}

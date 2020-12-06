using Chess.Domain.Entities;

namespace Chess.Models.ViewModels.PositionViewModels
{
    public class SolveProblemModel
    {
        /// <summary>
        /// Solution to the problem of the user that is currently signed in.
        /// </summary>
        public UserSolution UserSolution { get; set; }
        /// <summary>
        /// Position of the problem.
        /// </summary>
        public Position Position { get; set; }
    }
}
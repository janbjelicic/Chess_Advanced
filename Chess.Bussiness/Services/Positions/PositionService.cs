using Chess.Bussiness.Services.Enums;
using Chess.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Bussiness.Services.Positions
{
    public class PositionService
    {
        /// <summary>
        /// Filtering and ordering positions by the data set in the filter object.
        /// </summary>
        /// <param name="filter">Object that contains data according to which will the filtering be done.</param>
        /// <param name="positions">Positions that will be filtered and ordered.</param>
        /// <returns>Filtered and ordered positions.</returns>
        public static List<Position> FilterPositions(PositionFilter filter, IQueryable<Position> positions)
        {
            if (filter == null)
                return positions.ToList();
            switch (filter.Category)
            {
                case CategoryEnum.Fork:
                    positions = positions.Where(x => x.Category == CategoryEnum.Fork);
                    break;
                case CategoryEnum.Mate:
                    positions = positions.Where(x => x.Category == CategoryEnum.Mate);
                    break;
                case CategoryEnum.Pin:
                    positions = positions.Where(x => x.Category == CategoryEnum.Pin);
                    break;
                default:
                    break;
            }
            switch (filter.Difficulty)
            {
                case DifficultyEnum.Amateur:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.Amateur);
                    break;
                case DifficultyEnum.Beginner:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.Beginner);
                    break;
                case DifficultyEnum.Candidate:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.Candidate);
                    break;
                case DifficultyEnum.GrandMaster:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.GrandMaster);
                    break;
                case DifficultyEnum.Master:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.Master);
                    break;
                case DifficultyEnum.Unknown:
                    positions = positions.Where(x => x.Difficulty == DifficultyEnum.Unknown);
                    break;
                default:
                    break;
            }
            switch (filter.Solved)
            {
                case BoolEnum.True:
                    positions = positions.Where(x => x.UserSolutions.FirstOrDefault(y => y.User.ID == filter.UserID) != null
                        && x.UserSolutions.FirstOrDefault(y => y.User.ID == filter.UserID).IsSolved);
                    break;
                case BoolEnum.False:
                    positions = positions.Where(x => x.UserSolutions.FirstOrDefault(y => y.User.ID == filter.UserID) == null
                        || (x.UserSolutions.FirstOrDefault(y => y.User.ID == filter.UserID) != null
                        && !x.UserSolutions.FirstOrDefault(y => y.User.ID == filter.UserID).IsSolved));
                    break;
                default:
                    break;
            }
            
            
            // Order by part.
            switch (filter.SolvedTimes)
            {
                case(OrderByEnum.Ascending):
                    positions = positions.OrderBy(x => x.UserSolutions.Count(y => y.IsSolved));
                    break;
                case (OrderByEnum.Descending):
                    positions = positions.OrderByDescending(x => x.UserSolutions.Count(y => y.IsSolved));
                    break;
                default:
                    break;
            }

            switch (filter.SuccessRate)
            {
                case (OrderByEnum.Ascending):
                    positions = positions.OrderBy(x => (double)x.UserSolutions.Count(y => y.IsSolved) / (double)x.UserSolutions.Sum(y => y.NumberOfAttempts));
                    break;
                case (OrderByEnum.Descending):
                    positions = positions.OrderByDescending(x => (double)x.UserSolutions.Count(y => y.IsSolved) / (double)x.UserSolutions.Sum(y => y.NumberOfAttempts));
                    break;
                default:
                    break;
            }
            return positions.ToList();
        }
    }
}

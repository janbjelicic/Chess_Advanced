using Chess.Bussiness.Services.Positions;
using Chess.Domain.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Models.ViewModels.PositionViewModels
{
    public class IndexViewModel
    {
        /// <summary>
        /// Filter for positions on the index page.
        /// </summary>
        public PositionFilter Filter { get; set; }
        /// <summary>
        /// List of positions after they are filtered.
        /// </summary>
        public IPagedList<PositionDataViewModel> Positions { get; set; }
    }

    public class PositionDataViewModel
    {
        /// <summary>
        /// Position identitifer.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the position.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Rate of success on solving position.
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// Positions difficulty.
        /// </summary>
        public string Difficulty { get; set; }
        /// <summary>
        /// Positions category.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// How many times was the solution solved.
        /// </summary>
        public int Solved { get; set; }
        /// <summary>
        /// Is the problem solved by the currently logged in user.
        /// </summary>
        public bool IsSolved { get; set; }

        public static PositionDataViewModel MapToPositionDataViewModel(Position position, int userId)
        {
            var usersSolution = position.UserSolutions.FirstOrDefault(x => x.User.ID == userId);
            var rate = (double)position.UserSolutions.Sum(x => x.NumberOfAttempts) == 0
                ? 0 : Math.Round((double)position.UserSolutions.Count(x => x.IsSolved) / (double)position.UserSolutions.Sum(x => x.NumberOfAttempts) * 100, 2);
            return new PositionDataViewModel
            {
                ID = position.ID,
                Name = position.Name,
                Difficulty = position.Difficulty.ToString(),
                Category = position.Category.ToString(),
                Solved = position.UserSolutions.Count(x => x.IsSolved),
                Rate = rate,
                IsSolved = usersSolution != null ? usersSolution.IsSolved : false
            };
        }

        public static List<PositionDataViewModel> MapToPositionDataViewModels(List<Position> positions, int userId)
        {
            var positionDataViewModels = new List<PositionDataViewModel>();
            foreach (var position in positions)
            {
                positionDataViewModels.Add(MapToPositionDataViewModel(position, userId));
            }
            return positionDataViewModels;
        }
    }
}
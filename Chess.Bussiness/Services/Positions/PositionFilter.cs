using Chess.Bussiness.Services.Enums;
using Chess.Domain.Entities;
using Chess.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace Chess.Bussiness.Services.Positions
{
    public class PositionFilter
    {
        /// <summary>
        /// Difficulty of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Difficulty")]
        public DifficultyEnum Difficulty { get; set; }
        /// <summary>
        /// Positions category.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Category")]
        public CategoryEnum Category { get; set; }
        /// <summary>
        /// Solved or unsolved positions.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Solved")]
        public BoolEnum Solved { get; set; }
        /// <summary>
        /// Ordering by how many times has the position been solved.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Solved")]
        public OrderByEnum SolvedTimes { get; set; }
        /// <summary>
        /// Ordering by the success rate on solving a particular position.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Rate")]
        public OrderByEnum SuccessRate { get; set; }
        /// <summary>
        /// Identifier of the currently logged in user.
        /// </summary>
        public int UserID { get; set; }
    }
}

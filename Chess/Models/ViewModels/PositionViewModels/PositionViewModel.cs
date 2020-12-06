using Chess.Bussiness;
using Chess.Domain.Entities;
using Chess.Domain.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace Chess.Models.ViewModels.PositionViewModels
{
    public class PositionViewModel
    {
        /// <summary>
        /// Position identifier.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the position/problem.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Description of the position/problem.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        /// <summary>
        /// Date and time when the position was created.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// <summary>
        /// Describes the position of every piece on the board.
        /// </summary>
        public string PiecePositions { get; set; }
        /// <summary>
        /// If true white is on the move, otherwise black.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "WhiteIsPlaying")]
        public bool WhiteIsPlaying { get; set; }
        /// <summary>
        /// Difficulty of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Difficulty")]
        public DifficultyEnum Difficulty { get; set; }
        /// <summary>
        /// Position category.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Category")]
        public CategoryEnum Category { get; set; }
        /// <summary>
        /// List of moves that solve the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources), Name = "Solution")]
        public string Solution { get; set; }

        /// <summary>
        /// Mapping method from view model to entity model.
        /// </summary>
        /// <param name="position">Object that will be mapped.</param>
        /// <returns>New mapped object.</returns>
        public static Position MapToPosition(PositionViewModel position)
        {
            return new Position
            {
                ID = position.ID,
                WhiteIsPlaying = position.WhiteIsPlaying,
                Category = position.Category,
                DateCreated = position.DateCreated,
                Difficulty = position.Difficulty,
                Name = position.Name,
                Description = position.Description,
                PiecePositions = position.PiecePositions,
                Solution = MoveParser.ParseProblemMoves(position.Solution)
            };
        }
        /// <summary>
        /// Mapping method from entity model to view model.
        /// </summary>
        /// <param name="position">Object that will be mapped.</param>
        /// <returns>New mapped object.</returns>
        public static PositionViewModel MapToPositionViewModel(Position position)
        {
            return new PositionViewModel
            {
                ID = position.ID,
                WhiteIsPlaying = position.WhiteIsPlaying,
                Category = position.Category,
                Difficulty = position.Difficulty,
                Name = position.Name,
                DateCreated = position.DateCreated,
                Description = position.Description,
                PiecePositions = position.PiecePositions,
                Solution = MoveParser.StringBuildProblemMoves(position.Solution)
            };
        }
    }
}
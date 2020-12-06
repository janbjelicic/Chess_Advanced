using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Domain.Entities
{
    public enum DifficultyEnum
    {
        Unknown = 1,
        Beginner,
        Amateur,
        Candidate,
        Master,
        GrandMaster
    }

    public enum CategoryEnum
    {
        Mate = 1,
        Fork,
        Pin
    }

    [Table("Positions")]
    public class Position
    {
        /// <summary>
        /// Position identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Name of the position/problem.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Name")]
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Description of the position/problem.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public string Description { get; set; }
        /// <summary>
        /// Date and time when the position was created.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date and time when the position was updated.
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Describes the position of every piece on the board.
        /// </summary>
        [StringLength(100)]
        [Required]
        public string PiecePositions { get; set; }
        /// <summary>
        /// If true white is on the move, otherwise black.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "WhiteIsPlaying")]
        public bool WhiteIsPlaying { get; set; }
        /// <summary>
        /// Difficulty of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Difficulty")]
        public DifficultyEnum Difficulty { get; set; }
        /// <summary>
        /// Position category.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Category")]
        public CategoryEnum Category { get; set; }
        /// <summary>
        /// List of comments on the position.
        /// </summary>
        public List<Comment> Comments { get; set; }
        /// <summary>
        /// List of moves that solve the position.
        /// </summary>
        public List<Move> Solution { get; set; }
        /// <summary>
        /// List of solutions to problem by the users.
        /// </summary>
        public List<UserSolution> UserSolutions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Domain.Entities
{
    /// <summary>
    /// Used for tracking users solving the problems.
    /// </summary>
    [Table("UserSolution")]
    public class UserSolution
    {
        /// <summary>
        /// User solution identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Move on which the user is currently for the particular problem.
        /// </summary>
        public int CurrentMove { get; set; }
        /// <summary>
        /// Number of times when the user asnwered on the particular problem.
        /// </summary>
        public int NumberOfAttempts { get; set; }
        /// <summary>
        /// True if the problem is solved, false otherwise.
        /// </summary>
        public bool IsSolved { get; set; }
        /// <summary>
        /// Date when the user started solving the problem.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date when did the user last time tried to solve the problem.
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Identifier of the user which is solving the problem.
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// User which is solving the problem.
        /// </summary>
        [ForeignKey("UserID")]
        public User User { get; set; }
        /// <summary>
        /// Identifier of the problem being solved.
        /// </summary>
        public int ProblemID { get; set; }
        /// <summary>
        /// Problem which is being solved.
        /// </summary>
         [ForeignKey("ProblemID")]
        public Position Problem { get; set; }
    }
}

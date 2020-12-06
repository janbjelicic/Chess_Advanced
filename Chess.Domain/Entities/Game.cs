using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Domain.Entities
{
    public enum GameTypeEnum
    {
        Opening,
        Strategy,
        Ending
    }

    [Table("Games")]
    public class Game
    {
        /// <summary>
        /// Position identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Name of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Name")]
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        /// <summary>
        /// Description of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Description")]
        public string Description { get; set; }
        /// <summary>
        /// Code of the position.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Code")]
        [StringLength(10)]
        public string Code { get; set; }
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
        [Display(ResourceType = typeof(Resources.Resources), Name = "PiecePositions")]
        [StringLength(100)]
        [Required]
        public string PiecePositions { get; set; }
        /// <summary>
        /// Type of the game.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Type")]
        public GameTypeEnum Type { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Domain.Entities
{
    public enum PieceEnum
    {
        Unknown,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    [Table("Moves")]
    public class Move
    {
        /// <summary>
        /// Move identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Piece that was played on the move.
        /// </summary>
        public PieceEnum Piece { get; set; }
        /// <summary>
        /// Notational move description.
        /// </summary>
        [StringLength(20)]
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// Which is the move in the game.
        /// </summary>
        public int MoveNumber { get; set; }
        /// <summary>
        /// If true white played the move, otherwise black.
        /// </summary>
        public bool WhitePlayed { get; set; }
    }
}

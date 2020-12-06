using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Domain.Entities
{
    [Table("Matches")]
    public class Match
    {
        /// <summary>
        /// Match identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Describing the match played like : white, white_rating : black, black_rating score
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Player who plays the white pieces.
        /// </summary>
        public User White { get; set; }
        /// <summary>
        /// Player who plays the black pieces.
        /// </summary>
        public User Black { get; set; }
        /// <summary>
        /// When the match was played.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// When the match was updated, for example when a new comment
        /// is inserted for a match.
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Match result.
        /// </summary>
        [StringLength(10)]
        [Required]
        public string Result { get; set; }
        /// <summary>
        /// All the moves in the game.
        /// </summary>
        public List<Move> Moves { get; set; }
        /// <summary>
        /// Commentary on the game made by users.
        /// </summary>
        public List<Comment> Comments { get; set; }
        /// <summary>
        /// Suggests if the match was played on the web application,
        /// or did the administrator inserted it as notable old game
        /// between two masters.
        /// </summary>
        public bool IsPlayed { get; set; }
        /// <summary>
        /// Matches played by players can be rated or unrated.
        /// </summary>
        public bool IsRated { get; set; }
    }
}

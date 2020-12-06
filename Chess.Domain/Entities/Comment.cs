using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Domain.Entities
{
    [Table("Comments")]
    public class Comment
    {
        /// <summary>
        /// Comment identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// Content of the comment.
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// Date and time when the comment was created.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date and time when the comment was updated.
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Identifier of the user who wrote the comment.
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// User that wrote the comment.
        /// </summary>
        [ForeignKey("UserID")]
        public User User { get; set; }
        /// <summary>
        /// Match that was commented.
        /// </summary>
        public Match Match { get; set; }
        /// <summary>
        /// Position that was commented.
        /// </summary>
        public Position Position { get; set; }
    }
}

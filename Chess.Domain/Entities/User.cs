using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Domain.Entities
{
    public enum UserRoles
    {
        Administrator,
        Player
    }

    [Table("Users")]
    public class User
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// User's username.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "UserName")]
        [StringLength(20)]
        public string UserName { get; set; }
        /// <summary>
        /// User's first name
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "FirstName")]
        [StringLength(30)]
        public string FirstName { get; set; }
        /// <summary>
        /// User's last name.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "LastName")]
        [StringLength(30)]
        public string LastName { get; set; }
        /// <summary>
        /// User's address.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }
        /// <summary>
        /// Email of the user.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Confirmation token for email registration.
        /// </summary>
        [StringLength(50)]
        public string ConfirmationToken { get; set; }
        /// <summary>
        /// Rating that determines users strength when playing matches,
        /// against other users.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "PlayingRating")]
        public int PlayingRating { get; set; }
        /// <summary>
        /// Rating that determines users strength when solving chess problems.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "ProblemRating")]
        public int ProblemRating { get; set; }
        /// <summary>
        /// Date when the user was created.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date when the user was updated.
        /// </summary>
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        ///     Does the player want to play a
        ///     rated or unrated game.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "IsRated")]
        public bool IsRated { get; set; }

        /// <summary>
        ///     Minimum rating against the player will play.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "RatingFrom")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RatingFromRequiredErrorMessage")]
        [Range(0, 3500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RatingFromRangeErrorMessage")]
        public int RatingFrom { get; set; }

        /// <summary>
        ///     Maximum rating against the player will play.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "RatingTo")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RatingToRequiredErrorMessage")]
        [Range(0, 3500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "RatingToRangeErrorMessage")]
        public int RatingTo { get; set; }

        /// <summary>
        ///     How many minutes does each of the opponents has.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Minutes")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MinutesRequiredErrorMessage")]
        [Range(1, 120, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "MinutesRangeErrorMessage")]
        public int Minutes { get; set; }

        /// <summary>
        ///     How many seconds does each of the opponents gain per move.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Resources), Name = "Gain")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GainRequiredErrorMessage")]
        [Range(0, 30, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GainRangeErrorMessage")]
        public int Gain { get; set; }
        /// <summary>
        /// List of matches that the user played.
        /// </summary>
        public List<Match> Matches { get; set; }
        /// <summary>
        /// List of comments that the user has posted.
        /// </summary>
        public List<Comment> Comments { get; set; }
        /// <summary>
        /// List of solutions to problems by the user.
        /// </summary>
        public List<UserSolution> UserSolutions { get; set; }
        /// <summary>
        /// Which language does the user prefer.
        /// </summary>
        [StringLength(10)]
        public string CultureInfo { get; set; }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            if (user == null)
                return false;
            return user.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

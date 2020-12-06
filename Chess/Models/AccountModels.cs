using System.ComponentModel.DataAnnotations;
using Chess.Domain.Entities;
using Chess.Domain.Resources;

namespace Chess.Models
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources), Name = "UserName")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (Resources),
            ErrorMessageResourceName = "NewPasswordErrorMessage")]
        public string ConfirmPassword { get; set; }

        public User User { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources), Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources), Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(ResourceType = typeof (Resources), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "EmailErrorMessage", ErrorMessage = null)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources),
            ErrorMessageResourceName = "PasswordErrorMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (Resources),
            ErrorMessageResourceName = "PasswordConfirmErrorMessage")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
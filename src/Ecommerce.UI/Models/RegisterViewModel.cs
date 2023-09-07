using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.UI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O Email é obrigátorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Lembra-me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}

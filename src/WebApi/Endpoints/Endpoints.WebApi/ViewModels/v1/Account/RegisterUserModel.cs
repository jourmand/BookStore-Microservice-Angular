using System.ComponentModel.DataAnnotations;

namespace Endpoints.WebApi.ViewModels.v1.Account
{
    public class RegisterUserModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, 
         StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }


    }
}

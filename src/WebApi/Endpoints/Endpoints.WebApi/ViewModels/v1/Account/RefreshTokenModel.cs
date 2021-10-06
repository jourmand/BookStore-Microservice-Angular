using System.ComponentModel.DataAnnotations;

namespace Endpoints.WebApi.ViewModels.v1.Account
{
    public class RefreshTokenModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}

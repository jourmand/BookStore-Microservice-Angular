using System.ComponentModel.DataAnnotations;

namespace Endpoints.WebApi.ViewModels.v1.Subscribe
{
    public class SubscribeBookModel
    {
        [Required]
        public string BookId { get; set; }
    }
}

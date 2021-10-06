using System.ComponentModel.DataAnnotations;

namespace Endpoints.WebApi.ViewModels.v1.Book
{
    public class AddBookModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}

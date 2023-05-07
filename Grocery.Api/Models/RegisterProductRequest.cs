using Grocery.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace Grocery.Api.Models
{
    public class RegisterProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public ProductCategory Category { get; set; }
    }
}

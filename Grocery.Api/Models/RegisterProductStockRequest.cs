using GroceryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace Grocery.Api.Models
{
    public class RegisterProductStockRequest
    {
        [Required]
        public int TotalProducts { get; set; }
        [Required]
        public int ProductId { get; set; }       
        [Required]
        public bool WriteOff { get; set; }
    }
}

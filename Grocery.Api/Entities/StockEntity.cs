using Grocery.Api.Enums;
using GroceryApi.Base;
using GroceryApi.Entities;

namespace Grocery.Api.Entities
{
    public class StockEntity : BaseEntity
    {
        public int TotalProducts { get; set; }
        public bool WriteOff { get; set; }
        public DateTime? DateWriteOff { get; set; }
        public virtual UserEntity? User { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        public static StockEntity New(int totalProduct, int userId, int productId)
        {
            var stock = new StockEntity()
            {
                WriteOff = false,
                TotalProducts = totalProduct,
                DateWriteOff = null,
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.Now,
            };
            return stock;
        }

        internal void Edit(int totalProduct, int productId, bool writeOff)
        {
            TotalProducts = totalProduct;
            ProductId = productId;
            WriteOff = writeOff;
            if (writeOff)
            {
                DateWriteOff = DateTime.Now;
            }
           
            Updated();
        }
    }
}
using Grocery.Api.Enums;
using GroceryApi.Base;

namespace Grocery.Api.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }    
        public string Brand { get; set; }
        public string Price { get; set; }
        public ProductCategory Category { get; set; }

        /// <summary>
        /// Método para cadastrar novo produto
        /// </summary>
        /// <param name="name">nome do produto</param>
        /// <param name="brand">marca do produto</param>
        /// <param name="price">preço do produto</param>
        /// <param name="category">categoria do produto</param>
        /// <returns></returns>
        public static ProductEntity New(string name, string brand, string price, ProductCategory category)
        {
            var product = new ProductEntity()
            {
                Name = name,
                Brand = brand,
                Price = price,
                Category = category,
                CreatedAt = DateTime.Now,
            };
            return product;
        }

        internal void Edit(string name, string brand, string price, ProductCategory category)
        {
            Name = name;
            Brand = brand;
            Price = price;
            Category = category;

            Updated();
        }
        
    }
}

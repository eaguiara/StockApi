using Grocery.Api.Entities;
using Grocery.Api.Extensions;
using Grocery.Api.Models;
using GroceryApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opw.HttpExceptions;

namespace Grocery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly Context _db;

        public StockController(Context db)
        {
            _db = db;
        }

        /// <summary>
        /// Método que deleta o produto do estoque
        /// </summary>
        /// <param name="productStockId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpDelete("{productStockId}")]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<IActionResult> Delete(int productStockId)
        {
            var productStock = _db.Stocks
                .Where(s => s.Id == productStockId)
                .FirstOrDefault();

            if (productStock == null)
            {
                throw new NotFoundException($"Product on stock {productStockId} not exist");
            }

            productStock.Delete();

            _db.Stocks.Update(productStock);
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Método que edita o produto do estoque
        /// </summary>
        /// <param name="productStockId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpPut("{productStockId}")]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<IActionResult> Edit(int productStockId, RegisterProductStockRequest request)
        {
            var productStock = _db.Stocks
                 .SingleOrDefault(s => s.DeletedAt == null && s.Id == productStockId);

            if (productStock == null)
            {
                throw new NotFoundException($"Product stock {productStockId} not exist");
            }

            productStock.Edit(request.TotalProducts, request.ProductId, request.WriteOff);

            _db.Stocks.Update(productStock);
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        ///Método que adiciona produto no estoque
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<IActionResult> RegisterStock([FromBody] RegisterProductStockRequest request)
        {
            var userId = User.GetId();

            var newProductStock = StockEntity.New(request.TotalProducts, userId, request.ProductId);

            _db.Stocks.Add(newProductStock);
            await _db.SaveChangesAsync();

            newProductStock.TotalProducts++;
            return CreatedAtAction(null, new { newProductStock });
        }

        /// <summary>
        /// Método que obtem todos os produtos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Stock_Manager,Administrator,Stock_Analyst")]
        public async Task<IActionResult> Get()
        {
            var product = _db.Products.Where(p => p.DeletedAt == null);

            return Ok(product);
        }

        /// <summary>
        /// Método que obtem um produto do stock pelo ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpGet("{productStockId}")]
        [Authorize(Roles = "Stock_Manager,Administrator,Stock_Analyst")]
        public async Task<IActionResult> GetId(int productStockId)
        {
            var productStock = _db.Stocks
               .Where(s => s.Id == productStockId)
               .Include("Product")
               .Include("User")
               .FirstOrDefault();

            if (productStock == null)
            {
                throw new NotFoundException($"Product {productStockId} not exist");
            }
            return Ok(productStock);
        }
    }
}
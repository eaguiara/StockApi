using Grocery.Api.Entities;
using Grocery.Api.Models;
using GroceryApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Opw.HttpExceptions;
using System.Data;
using System.Web.Http.OData;

namespace Grocery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Context _db;

        public ProductController(Context db)
        {
            _db = db;
        }

        /// <summary>
        /// Método que deleta o usuário
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpDelete("{productId}")]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            var product = _db.Products
                .SingleOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException($"Product {productId} not exist");
            }

            product.Delete();

            _db.Products.Update(product);
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Método que edita um produto
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpPut("{productId}")]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<IActionResult> Edit(int? productId, RegisterProductRequest request)
        {
            var product = _db.Products
            .SingleOrDefault(p => p.DeletedAt == null && p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException($"Product {productId} not exist");
            }

            product.Edit(request.Name, request.Brand, request.Price, request.Category);

            _db.Products.Update(product);
            await _db.SaveChangesAsync();

            return Ok();
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
        /// Método que obtem um produto pelo ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpGet("{productId}")]
        [Authorize(Roles = "Stock_Manager,Administrator,Stock_Analyst")]
        public async Task<IActionResult> GetId(int productId)
        {
            var product = _db.Products
                .SingleOrDefault(p => p.DeletedAt == null && p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException($"Product {productId} not exist");
            }
            return Ok(product);
        }

        /// <summary>
        /// Método que adidiona o produto na base de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns>  </returns>
        ///
        [HttpPost]
        [Authorize(Roles = "Stock_Manager,Administrator")]
        public async Task<ActionResult> RegisterProduct([FromBody] RegisterProductRequest request)
        {
            var newProduct = ProductEntity.New(request.Name, request.Brand, request.Price, request.Category);

            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();

            return CreatedAtAction(null, new { newProduct });
        }
    }
}
using Azure.Core;
using Grocery.Api.Enums;
using Grocery.Api.Models;
using GroceryApi.Data;
using GroceryApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Opw.HttpExceptions;
using System;
using System.Data;
using System.Text;

namespace Grocery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _db;

        public UserController(Context db)
        {
            _db = db;
        }

        /// <summary>
        /// Método que edita o usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int userId, RegisterUserRequest request)
        {
            var user = _db.Users
                 .SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            user.Edit(request.Name, request.Username, request.Password);

            _db.Users.Update(user);
            _db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Método que deleta o usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int userId)
        {
            var user = _db.Users
                 .SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"User {userId} not exist");
            }

            if (user.Id == 0)
            {
                throw new Exception("Invalid");
            }
            else
            {
                user.Delete();

                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }

        /// <summary>
        /// Método que obtem todos os usuarios cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var user = _db.Users.Where(p => p.DeletedAt == null);

            return Ok(user);
        }

        /// <summary>
        /// Método que obtem um usuario pelo ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetId(int userId)
        {
            var user = _db.Users
               .Where(s => s.Id == userId)
               .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundException($"User {userId} not exist");
            }
            return Ok(user);
        }
    }
}
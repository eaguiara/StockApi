using Grocery.Api.Enums;
using GroceryApi.Base;
using Newtonsoft.Json;

namespace GroceryApi.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public UserRoles Role { get; set; }

        /// <summary>
        /// Método para alterar a senha do usuário
        /// </summary>
        /// <param name="newPassword">Nova senha</param>
        /// <exception cref="Exception">Caso tenha uma falha na biblioteca de hashing</exception>
        public void ChangePassword(string newPassword)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            if (hash == null)
            {
                throw new Exception("Error on hash password");
            }
            PasswordHash = hash;
        }

        /// <summary>
        /// Método para validar a senha do usuário
        /// </summary>
        /// <param name="providedPassword">Senha provida pelo usuário</param>
        /// <returns>True caso a senha esteja igual e False caso esteja diferente</returns>
        public bool ValidatePassword(string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, PasswordHash);
        }

        /// <summary>
        /// Método para criar novo usuário
        /// </summary>
        /// <param name="name">Nome do usuário</param>
        /// <param name="userName"> username do usuário</param>
        /// <param name="password"> Senha do usuário</param>
        /// <returns>Novo usuário</returns>
        public static UserEntity New(string name, string userName, string password, UserRoles role)
        {
            var user = new UserEntity()
            {
                Username = userName,
                Name = name,
                CreatedAt = DateTime.Now,
                Role = role,
            };
            user.ChangePassword(password);

            return user;
        }

        /// <summary>
        /// Método que faz o update
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void Edit(string name, string userName, string password)
        {
            Username = userName;
            Name = name;
            ChangePassword(password);
        }
    }
}
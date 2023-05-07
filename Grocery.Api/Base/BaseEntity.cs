using System.IO.Pipes;

namespace GroceryApi.Base
{
    /// <summary>
    /// Classe que define o padrão de todas as entidades
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Método que inclui a data atual em que a propriedade foi atualizada
        /// </summary>
        public void Updated()
        {
            UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Método que inclui a data atual em que a propriedade foi deletada
        /// </summary>
        public void Delete()
        {
            DeletedAt = DateTime.Now;
        }
    }


}

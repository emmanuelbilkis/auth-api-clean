using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApi.Models.Db
{
    public class ActivationTokenModel
    {
        [Key]
        public int Id { get; set; }
        public UserModel User { get; set; }  // Propiedad de navegación: muchos a uno
        public int UserId { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
       
        
    }
}

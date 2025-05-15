using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class ActivationTokenModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; } 
        public bool Active { get; set; }    
    }
}

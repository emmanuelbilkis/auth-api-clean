using AuthApi.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.Db
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age
        {
            get
            {
                //return DateUtils.AgeCalculate(DateOfBirth); //sin metodo de extension
                return DateOfBirth.AgeCalculateExtension(); // con metodo de extension
            }
        }
        public bool IsActive { get; set; } = false;
        
        // Propiedad de navegación: uno a muchos (un usuario puede tener varios tokens)
        public ICollection<ActivationTokenModel> ActivationTokens { get; set; } = new List<ActivationTokenModel>(); 
    }
}

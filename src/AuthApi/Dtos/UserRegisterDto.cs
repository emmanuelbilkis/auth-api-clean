namespace AuthApi.Dtos
{
    public class UserRegisterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }  
        public string Password  { get; set; }
        public string PasswordConfirmed { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

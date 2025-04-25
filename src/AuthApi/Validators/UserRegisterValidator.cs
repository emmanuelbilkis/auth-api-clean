using AuthApi.Dtos;
using FluentValidation;
using System.Data;
using System.Security.Cryptography.Xml;

namespace AuthApi.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator() 
        {
            // Validación de Nombre
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Debe ingresar un valor válido de nombre")
                .NotEmpty().WithMessage("Debe ingresar un nombre");

            // Validación de Apellido
            RuleFor(x => x.LastName)
                .NotNull().WithMessage("Debe ingresar un valor válido de apellido")
                .NotEmpty().WithMessage("Debe ingresar un apellido");
                
            // Validación de Correo
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Debe ingresar un valor válido de correo")
                .NotEmpty().WithMessage("Debe ingresar un correo")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Debe ingresar un formato válido de correo electrónico.");

            // Validación de repetición del correo
            RuleFor(x => x.EmailConfirmed)   
             .Equal(x => x.Email).WithMessage("El correo de confirmación debe ser igual al correo ingresado.");
               
            // Validacion de la contrasena
            RuleFor(x => x.Password).NotNull().WithMessage("Debe ingresar un valor válido de contrasena")
            .NotEmpty().WithMessage("Debe ingresar una contrasena")
            .MinimumLength(10).WithMessage("Debe ingresar como minimo 10 caracteres para la contrasena")
            .Matches(@"[A-Z]").WithMessage("La contrasena debe contener al menos una letra mayúscula.")
            .Matches(@"[0-9]").WithMessage("La contrasena debe contener al menos un número.")
            .Matches(@"[\W]").WithMessage("La contrasena debe contener al menos un carácter especial.")
            .NotEqual(x => x.Email).WithMessage("No debe ingresar una contrasena  que contenga informacion conocida")
            .NotEqual(x => x.Name).WithMessage("No debe ingresar una contrasena  que contenga informacion conocida")
            .NotEqual(x => x.LastName).WithMessage("No debe ingresar una contrasena  que contenga informacion conocida")
            .NotEqual(x => x.DateOfBirth.ToString()).WithMessage("No debe ingresar una contrasena  que contenga informacion conocida");

            // Valdiacion de la repeticion de la contrasena
            RuleFor(x => x.Password)
            .Equal(x => x.Password).WithMessage("La contrasena de confirmacion debe ser igual a la contrasena ingresada");
        }
    }
}

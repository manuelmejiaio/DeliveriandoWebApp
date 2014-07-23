using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace DeliveriandoWebApp.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<UsersContext>(null);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        // Customization: Field(s) added
        public int RestauranteID { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserCreditCard { get; set; }
        public string UserFullName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        //[Required]
        [Required(ErrorMessage = "La actual Contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        //[Required]
        [Required(ErrorMessage = "La nueva Contraseña es requerida.")]
        [StringLength(100, ErrorMessage = "La {0} debe contener {2} caracteres al menos.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "nueva Contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación  deben ser iguales.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        //[Required]
        [Required(ErrorMessage = "El Usuario es requerido.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        //[Required]
        [Required(ErrorMessage = "La Contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Recuérdame")]
        public bool RememberMe { get; set; }

        
    }

    public class RegisterModel
    {
        //[Required]
        [Required(ErrorMessage = "El Correo Electrónico es requerido. ")]
        [Display(Name = "Your email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El Correo Electrónico no es valido. ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Su nombre completo es requerido. ")]
        [Display(Name = "User full name")]
        public string UserFullName { get; set; }

        //[Required]
        [Required(ErrorMessage = "La Contraseña es requerida.")]
        [StringLength(100, ErrorMessage = "La {0} debe contener {2} caracteres al menos. ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación  deben ser iguales. ")]
        public string ConfirmPassword { get; set; }

        // Customization: Field(s) added
        [Required]
        [Display(Name = "RestauranteID")]
        [DataType(DataType.Text)]
        public int RestauranteID { get; set; }

        [Required(ErrorMessage = "Su dirección de residencia es requerida. ")]
        [Display(Name = "Your address")]
        [DataType(DataType.Text)]
        public string UserAddress { get; set; }

        [Required(ErrorMessage = "Su numero de teléfono residencial es requerido.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[. ]?([0-9]{3})[. ]?([0-9]{4})$", ErrorMessage = "Debe contener 10 numeros. ")]
        [Display(Name = "Your phone")]
        [DataType(DataType.PhoneNumber)]
        public string UserPhone { get; set; }

        [RegularExpression(@"^\(?([0-9]{4})\)?[. ]?([0-9]{4})[. ]?([0-9]{8})$", ErrorMessage = "La tarjeta debe contener 16 numeros. ")]
        [Display(Name = "Your Credit Card")]
        [DataType(DataType.CreditCard)]
        public string UserCreditCard { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_ecommerce_v1.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'name' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'name' no debe contener números.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "El campo 'last name' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'last name' no debe contener números.")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "El campo 'email' es requerido.")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "El campo 'email' debe ser una dirección de correo electrónico válida.")]
        public string Email { get; set;}

        [Required]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'country' no debe contener números.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "El campo 'password' es requerido.")]
        [StringLength(200)]
        public string Password { get; set;}

        [Required]
        [StringLength(50)]
        public string Profile { get; set;}

        [Required(ErrorMessage = "El campo 'phone' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'phone' debe ser un número.")]
        public int phone { get; set;}

        [Required]
        [StringLength(50)]
        public string Fbirth { get; set;}

        [Required]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'gender' no debe contener números.")]
        public string gender { get; set;}

        [Required(ErrorMessage = "El campo 'nit' es requerido.")]
        [StringLength(50)]
        public string Nit { get; set;}
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class NotNumericAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true; // Permitir valores nulos
        }

        if (value is string strValue && !strValue.Any(char.IsDigit))
        {
            return true; // No contiene dígitos numéricos, válido
        }

        return false;
    }
}
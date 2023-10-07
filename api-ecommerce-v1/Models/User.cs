using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class User
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

        [Required(ErrorMessage = "El campo 'address' es requerido.")]
        [StringLength(50)]
        public string address { get; set; }

        [Required(ErrorMessage = "El campo 'phone' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'phone' debe ser un número.")]
        public int phone { get; set;}

        [Required(ErrorMessage = "El campo 'nit' es requerido.")]
        [StringLength(50)]
        public string nit { get; set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public User()
        {
            createdDate = DateTime.Now;
        }

        /*
         * Relación con la tabla Login 
        */
        public int LoginId { get; set; }
        public Login? Login { get; set; }
    }

}

/*
 * Validación personalizada para que el campo no contenga números, solo letras
 * utilizando expresiones regulares y atributos para la validación
 */

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
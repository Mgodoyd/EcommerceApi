using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Admin
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


        [Required(ErrorMessage = "El campo 'phone' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'phone' debe ser un número.")]
        public int phone { get; set; }

        [Required(ErrorMessage = "El campo 'nit' es requerido.")]
        [StringLength(50)]
        public string nit { get; set; }


        [Required(ErrorMessage = "El campo 'rol' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'rol' no debe contener números.")]
        public string rol { get; set; }

        // Propiedad de navegación para la relación con Login
        public int LoginId { get; set; }
        public Login Login { get; set; }
    }
}


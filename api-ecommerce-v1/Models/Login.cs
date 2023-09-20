using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api_ecommerce_v1.Models
{
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'email' es requerido.")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "El campo 'email' debe ser una dirección de correo electrónico válida.")]
        public string email { get; set; }

        [Required(ErrorMessage = "El campo 'password' es requerido.")]
        [StringLength(200)]
        public string password { get; set; }


        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'rol' no debe contener números.")]
        public string? rol { get; set; }


        [JsonIgnore]
        public  User? user { get; set; }
     }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_ecommerce_v1.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'customer' es requerido.")]
        [StringLength(50)]
        public string customer { get; set; }


        [Required(ErrorMessage = "El campo 'message' es requerido.")]
        [StringLength(50)]
        public string message { get; set; }


        [Required(ErrorMessage = "El campo 'subject' es requerido.")]
        [StringLength(50)]
        public string subject { get; set; }


        [Required(ErrorMessage = "El campo 'email' es requerido.")]
        [StringLength(50)]
        public string email { get; set; }

        [Required(ErrorMessage = "El campo 'phone' es requerido.")]
        public int phone { get; set; }

        [StringLength(50)]
        public string? state { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public Contact()
        {
            // Inicializa createdDate con la fecha y hora actuales del servidor
            createdDate = DateTime.Now; // Utiliza DateTime.UtcNow si prefieres la hora UTC
        }
    }
}

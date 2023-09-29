using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Sales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'nsale' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'nsale' no debe contener números.")]
        public string nsale { get; set; }

        [Required(ErrorMessage = "El campo 'subtotal' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'subtotal' debe ser un número.")]
        public int subtotal { get; set; }

        [Required(ErrorMessage = "El campo 'envio_title' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'envio_title' no debe contener números.")]
        public string envio_title { get; set; }


        [Required(ErrorMessage = "El campo 'envio_price' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'envio_price' debe ser un número.")]
        public int envio_price { get; set; }

        [Required]
        [StringLength(50)]
        public string transaction { get; set; }


        [StringLength(50)]
        public string? coupon { get; set; }

        [Required(ErrorMessage = "El campo 'state' es requerido.")]
        [StringLength(50)]
        public string state { get; set; }

        [Required(ErrorMessage = "El campo 'note' es requerido.")]
        [StringLength(50)]
        public string note { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public Sales()
        {
            // Inicializa createdDate con la fecha y hora actuales del servidor
            createdDate = DateTime.Now; // Utiliza DateTime.UtcNow si prefieres la hora UTC
        }

        // Propiedad de navegación para la relación con Login
        public int userId { get; set; }
        public User? user { get; set; }

        public int addressId { get; set; }
        public Address? address { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class NSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'subtotal' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'subtotal' debe ser un número.")]
        public int subtotal { get; set; }


        [Required(ErrorMessage = "El campo 'envio_price' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'envio_price' debe ser un número.")]
        public int amount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public NSale()
        {
            // Inicializa createdDate con la fecha y hora actuales del servidor
            createdDate = DateTime.Now; // Utiliza DateTime.UtcNow si prefieres la hora UTC
        }

        public int productId { get; set; }
        public List<Product>? products { get; set; }

        public int userId { get; set; }
        public List<User>? users { get; set; }
    }
}

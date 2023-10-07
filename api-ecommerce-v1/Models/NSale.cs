using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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


        [Required(ErrorMessage = "El campo 'amount' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'amount' debe ser un número.")]
        public int amount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public NSale()
        {
            createdDate = DateTime.Now;
        }

        /*
         * Relacion con la tabla Product
         */
        public int productId { get; set; }
        public Product? products { get; set; }

        /*
         * Relacion con la tabla User
        */
        public int userId { get; set; }
        public List<User>? users { get; set; }

        /*
         *   Relacion con la tabla Sales
        */
        public int SalesId { get; set; }

        [JsonIgnore]
        public Sales? sales { get; set; }
    }
}

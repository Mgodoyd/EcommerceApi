using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Cart
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required(ErrorMessage = "El campo 'stock' es requerido.")]
            [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'stock' debe ser un número.")]
            public int? amount { get; set; }

            public int productId { get; set; }
            public Product? products { get; set; }
            public ICollection<Product>? product { get; set; }

            public int userId { get; set; }
            public User? users { get; set; }
            public ICollection<User>? user { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public DateTime createdDate { get; set; }

            public Cart()
            {
                createdDate = DateTime.Now;
            }
        }
    }


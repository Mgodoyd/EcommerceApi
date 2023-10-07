using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Coupon
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

            public int Id { get; set; }

            [Required(ErrorMessage = "El campo 'code' es requerido.")]
            [StringLength(50)]
            public string code { get; set; }


            [Required(ErrorMessage = "El campo 'type' es requerido.")]
            [StringLength(50)]
            public string type { get; set; }


            [Required(ErrorMessage = "El campo 'value' es requerido.")]
            public int value { get; set; }


            [Required(ErrorMessage = "El campo 'limit' es requerido.")]
            [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'limit' debe ser un número.")]
            public int limit { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public DateTime createdDate { get; set; }

            public Coupon()
            {
                createdDate = DateTime.Now;
            }
        }

    }



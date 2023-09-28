using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Address
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required(ErrorMessage = "El campo 'addressee' es requerido.")]
            [StringLength(50)]
            [NotNumeric(ErrorMessage = "El campo 'addressee' no debe contener números.")]
            public string addressee { get; set; }


            [Required(ErrorMessage = "El campo 'dpi' es requerido.")]
            public int? dpi { get; set; }


            [Required(ErrorMessage = "El campo 'zip' es requerido.")]
            [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'zip' debe ser un número.")]
            public int zip { get; set; }

            [Required(ErrorMessage = "El campo 'country' es requerido.")]
            [StringLength(50)]
            [NotNumeric(ErrorMessage = "El campo 'country' no debe contener números.")]
            public string country { get; set; }

            [Required(ErrorMessage = "El campo 'phone' es requerido.")]
            public int phone { get; set; }

            public Boolean main { get; set; }

            public int userId { get; set; }
            public User? user { get; set; }
            //public ICollection<User>? user { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public DateTime createdDate { get; set; }

            public Address()
            {
                // Inicializa createdDate con la fecha y hora actuales del servidor
                createdDate = DateTime.Now; // Utiliza DateTime.UtcNow si prefieres la hora UTC
            }

           
        }

    }



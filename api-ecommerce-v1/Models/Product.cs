using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api_ecommerce_v1.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'titulo' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'titulo' no debe contener números.")]
        public string title { get; set; }
        public string? frontpage { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "El campo 'price' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'price' debe ser un número.")]
        public int price { get; set; }


        [Required(ErrorMessage = "El campo 'description' es requerido.")]
        [StringLength(50)]
        public string description { get; set; }

        [Required(ErrorMessage = "El campo 'content' es requerido.")]
        [StringLength(50)]
        public string content { get; set; }

        [Required(ErrorMessage = "El campo 'stock' es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'stock' debe ser un número.")]
        public int stock { get; set; }

        /*
         * Relación con la tabla Inventory  
         */
        public int inventoryId { get; set; }

        [JsonIgnore]
        public Inventory? inventory { get; set; }

        /*
         *   Relación con la tabla Category  
        */
        public int categoryId { get; set; }
        public Category? category { get; set; }

        /*
         *  Relación con la tabla Galery  
        */

        //public Galery? galerys { get; set; }
        public ICollection<Galery>? Galerys { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdDate { get; set; }

        public Product()
        {
            createdDate = DateTime.Now; 
        }
    }
}

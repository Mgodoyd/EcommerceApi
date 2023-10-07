using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_ecommerce_v1.Models
{
    public class Galery
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        [StringLength(50)]
        public string? galery { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        /*
         * Relación a la tabla Product
        */
        public int productId { get; set; }
        [JsonIgnore]
        public Product? product { get; set; }
    }
}

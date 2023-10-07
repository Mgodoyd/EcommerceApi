using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce_v1.Models
{
    public class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'title' es requerido.")]
        [StringLength(50)]
        [NotNumeric(ErrorMessage = "El campo 'title' no debe contener números.")]
        public string title { get; set; }


        [StringLength(100)]
        public string? logo { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        [Required(ErrorMessage = "El campo 'serie' es requerido.")]
        public string serie { get; set; }

        [Required(ErrorMessage = "El campo 'correlative' es requerido.")]
        public string correlative { get; set; }


        /*   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
           public DateTime createdDate { get; set; }

           public Config()
           {
               createdDate = DateTime.Now; 
           }*/
    }
}

using System.ComponentModel.DataAnnotations;

namespace Lab07_EstebanPacheco.DTO
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
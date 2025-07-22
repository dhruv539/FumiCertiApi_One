using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public decimal ProductOpening { get; set; }
        public string ProductOpeningUnit { get; set; }
        public decimal ProductWeightPerUnit { get; set; }
        public decimal ProductTotalWt { get; set; }
        public decimal ProductConsumeQty { get; set; }
    }

    public class ProductAddDto
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductUnit { get; set; }
        public decimal ProductOpening { get; set; }
        public string ProductOpeningUnit { get; set; }
        public decimal ProductWeightPerUnit { get; set; }
        public decimal ProductTotalWt { get; set; }
        public decimal ProductConsumeQty { get; set; }
    }

}

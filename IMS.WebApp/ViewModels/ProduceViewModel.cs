using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class ProduceViewModel
    {

        [Required]
        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        [ProduceEnsureEnoughInventoryQuantity]
        public int QuantityToProduce { get; set; }

        public Product? Product { get; set; } = null;

    }
}

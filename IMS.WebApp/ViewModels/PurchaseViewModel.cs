using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseViewModel
    {

        [Required]
        public string PONumber { get; set; } = string.Empty;

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int InventoryId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int QuantityToPurchase { get; set; }

        public double InventoryPrice { get; set; }

    }
}

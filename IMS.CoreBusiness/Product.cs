using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        [ProductEnsurePriceIsGreaterThanInventoriesCost]
        public List<ProductInventory> ProductInventories { get; set; } = new();

        public void AddInventory(Inventory inventory)
        {
            if (!ProductInventories.Any(x => x.Inventory != null && x.Inventory.InventoryName.Equals(inventory.InventoryName)))
            {
                ProductInventories.Add(new ProductInventory
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    InventoryQuantity = 1,
                    ProductId = ProductId,
                    Product = this

                });
            }
        }
    }
}

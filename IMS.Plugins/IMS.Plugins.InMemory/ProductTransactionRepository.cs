using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private List<ProductTransaction> _productTransactions = new();

        public ProductTransactionRepository(IInventoryRepository inventoryRepository, IProductRepository productRepository, IInventoryTransactionRepository inventoryTransactionRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? productTransactionType)
        {
            var products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();


            var query = from it in _productTransactions
                        join prod in products on it.ProductId equals prod.ProductId
                        where
                        (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date)
                        &&
                        (!dateTo.HasValue || it.TransactionDate >= dateTo.Value.Date)
                        &&
                        (!productTransactionType.HasValue || it.ActivityType == productTransactionType)
                        select new ProductTransaction()
                        {
                            Product = prod,
                            ProductTransactionId = it.ProductTransactionId,
                            SONumber = it.SONumber,
                            ProductionNumber = it.ProductionNumber,
                            ProductId = it.ProductId,
                            QuantityBefore = it.QuantityBefore,
                            ActivityType = it.ActivityType,
                            QuantityAfter = it.QuantityAfter,
                            TransactionDate = it.TransactionDate,
                            DoneBy = it.DoneBy,
                            UnitPrice = it.UnitPrice,
                        };
            return query;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            var prod = await _productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    if (pi.Inventory != null)
                    {
                        await _inventoryTransactionRepository.ProduceAsync(productionNumber,
                             pi.Inventory, pi.InventoryQuantity * quantity, doneBy, -1);
                    }

                    var inv = await _inventoryRepository.GetInventoriesByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await _inventoryRepository.EditInventoryAsync(inv);
                }
            }

            _productTransactions.Add(new ProductTransaction()
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,

            });

        }

        public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            _productTransactions.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SONumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });
            return Task.CompletedTask;
        }
    }
}

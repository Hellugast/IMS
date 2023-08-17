﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {

        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product() {ProductId = 1 , ProductName = "Bike", Quantity = 10 , Price = 150},
                new Product() {ProductId = 2 , ProductName = "Car", Quantity=5 , Price = 25000}
            };
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
                return Task.CompletedTask;

            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId + 1;

            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task EditProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductId != product.ProductId && x.ProductName.ToLower() == product.ProductName.ToLower()))
                return Task.CompletedTask;

            var prod = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
                prod.ProductInventories = product.ProductInventories;
            }
            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var prod = await Task.FromResult(_products.FirstOrDefault(x => x.ProductId == productId, new()));
            Product newProd = new();
            if (prod != null)
            {
                newProd = new Product()
                {
                    ProductId = productId,
                    ProductName = prod.ProductName,
                    Quantity = prod.Quantity,
                    Price = prod.Price,
                    ProductInventories = new()
                };
                if (prod.ProductInventories != null && prod.ProductInventories.Count > 0)
                {
                    foreach (var prodInv in prod.ProductInventories)
                    {
                        var newProdInv = new ProductInventory()
                        {
                            InventoryId = prodInv.InventoryId,
                            ProductId = prodInv.ProductId,
                            Product = prod,
                            Inventory = new(),
                            InventoryQuantity = prodInv.InventoryQuantity
                        };
                        if (prodInv.Inventory != null)
                        {
                            newProdInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
                            newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                            newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                            newProdInv.Inventory.Price = prodInv.Inventory.Price;
                        }

                        newProd.ProductInventories.Add(newProdInv);
                    }
                }

            };

            return await Task.FromResult(newProd);

        }




        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));

        }

    }
}

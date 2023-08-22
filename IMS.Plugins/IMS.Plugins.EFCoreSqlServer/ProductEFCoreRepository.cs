using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> _dbContextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> dbContext)
        {
            _dbContextFactory = dbContext;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = _dbContextFactory.CreateDbContext();

            db.Products.Add(product);
            FlagInventoryUnchanged(product, db);

            await db.SaveChangesAsync();
        }

        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
            if (product?.ProductInventories != null && product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null)
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                }
            }
        }

        public async Task EditProductAsync(Product product)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var prod = await db.Products.Include(p => p.ProductInventories)
                .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Quantity = product.Quantity;
                prod.Price = product.Price;
                prod.ProductInventories = product.ProductInventories;

                FlagInventoryUnchanged(product, db);

                await db.SaveChangesAsync();
            }

        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return await db.Products.Include(p => p.ProductInventories)
                .ThenInclude(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }




        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return await db.Products.Where(p => p.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

    }
}

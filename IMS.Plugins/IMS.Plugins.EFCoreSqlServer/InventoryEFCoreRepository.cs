using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSContext> _dbContextFactory;

        public InventoryEFCoreRepository(IDbContextFactory<IMSContext> dbContext)
        {
            _dbContextFactory = dbContext;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            using var db = _dbContextFactory.CreateDbContext();
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();
        }

        public async Task EditInventoryAsync(Inventory inventory)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var inv = await db.Inventories.FindAsync(inventory.InventoryId);
            if (inv != null)
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;

                await db.SaveChangesAsync();
            }

        }

        public async Task<Inventory> GetInventoriesByIdAsync(int inventoryId)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var result = await db.Inventories.FindAsync(inventoryId);
            if (result != null) return result;
            return new();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            using var db = _dbContextFactory.CreateDbContext();
            return await db.Inventories.Where(
                inv => inv.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }
    }
}
using CatsyTest.Data;
using CatsyTest.Model;
using Microsoft.EntityFrameworkCore;

namespace CatsyTest.services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CatsyTestContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Product>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(ProductRepository));
                return new List<Product>();
            }
        }
        public override async Task<bool> Upsert(Product entity)
        {
            try
            {
                var product = await dbSet.Where(x => x.ProductId == entity.ProductId)
                                                    .FirstOrDefaultAsync();

                if (product == null)
                    return await Add(entity);

                product.ProductName = entity.ProductName;
                product.ProductPrice = entity.ProductPrice;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(ProductRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.ProductId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(ProductRepository));
                return false;
            }
        }
    }
}

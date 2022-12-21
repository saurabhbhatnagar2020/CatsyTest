using CatsyTest.Data;
using CatsyTest.Model;
using Microsoft.EntityFrameworkCore;

namespace CatsyTest.services
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(CatsyTestContext context, ILogger logger) : base(context, logger)
        {
        }


        public async Task<IEnumerable<OrderDetails>> GetOrderDetails(int orderId)
        {
            return await dbSet.Include("Product").Where(c => c.OrderId == orderId).ToListAsync();
        }

        public override async Task<IEnumerable<OrderDetails>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(OrderDetailsRepository));
                return new List<OrderDetails>();
            }
        }
        public override async Task<bool> Upsert(OrderDetails entity)
        {
            try
            {
                var order = await dbSet.Where(x => x.OrderId == entity.OrderId)
                                                    .FirstOrDefaultAsync();

                if (order == null)
                    return await Add(entity);

                order.OrderDetailsId = entity.OrderDetailsId;

                order.OrderId = entity.OrderId;
                order.ProductId = entity.ProductId;
                order.Price = entity.Price;
                entity.Quantity = entity.Quantity;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(OrderDetailsRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.OrderId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(OrderDetailsRepository));
                return false;
            }
        }
    }
}

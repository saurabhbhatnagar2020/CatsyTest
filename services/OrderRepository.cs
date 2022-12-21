using CatsyTest.Data;
using CatsyTest.Model;
using Microsoft.EntityFrameworkCore;

namespace CatsyTest.services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CatsyTestContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Order>> All()
        {
            try
            {
                var result =  await dbSet.Include("Customer").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(OrderRepository));
                return new List<Order>();
            }
        }

        public override async Task<Order> GetById(int id)
        {
            try
            {
                var result = await dbSet.Include("Customer").Where(c=>c.OrderId == id).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(OrderRepository));
                return new Order();
            }
        }
        public override async Task<bool> Upsert(Order entity)
        {
            try
            {
                var order = await dbSet.Where(x => x.OrderId == entity.OrderId)
                                                    .FirstOrDefaultAsync();

                if (order == null)
                    return await Add(entity);

                order.OrderDate = entity.OrderDate;

                order.OrderId = entity.OrderId;
                order.CustomerId = entity.CustomerId;
                order.OrderDetails = entity.OrderDetails;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(OrderRepository));
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
                _logger.LogError(ex, "{Repo} Delete function error", typeof(OrderRepository));
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetOrderDetailsByCustomerId(int customerId)
        {
            try
            {
                var result = await dbSet.Include("Customer").Where(c => c.CustomerId == customerId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(OrderRepository));
                return new List<Order>();
            }
        }
    }
}

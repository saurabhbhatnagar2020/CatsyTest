using CatsyTest.Model;

namespace CatsyTest.services
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrderDetailsByCustomerId(int customerId);
    }
}

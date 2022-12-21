using CatsyTest.Model;

namespace CatsyTest.services
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        Task<IEnumerable<OrderDetails>> GetOrderDetails(int orderId);
    }
}

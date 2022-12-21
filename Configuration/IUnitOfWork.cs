using CatsyTest.services;

namespace CatsyTest.Configuration
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customer { get; }
        IProductRepository Product { get; }
        IOrderRepository Orders { get; }
        IOrderDetailsRepository OrderDetails { get; }
        Task CompleteAsync();
        void Dispose();
    }
}

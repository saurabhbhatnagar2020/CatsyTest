using CatsyTest.Data;
using CatsyTest.services;

namespace CatsyTest.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CatsyTestContext _context;
        private readonly ILogger _logger;

        public ICustomerRepository Customer { get; private set; }
        public IProductRepository Product { get; private set; }

        public IOrderRepository Orders   { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }
        public UnitOfWork(CatsyTestContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Customer = new CustomerRepository(context, _logger);
            Product = new ProductRepository(context, _logger);
            Orders = new OrderRepository(context, _logger);
            OrderDetails = new OrderDetailsRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using CatsyTest.Data;
using CatsyTest.Model;
using Microsoft.EntityFrameworkCore;

namespace CatsyTest.services
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CatsyTestContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Customer>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(CustomerRepository));
                return new List<Customer>();
            }
        }
        public override async Task<bool> Upsert(Customer entity)
        {
            try
            {
                var existingUser = await dbSet.Where(x => x.CustomerId == entity.CustomerId)
                                                    .FirstOrDefaultAsync();

                if (existingUser == null)
                    return await Add(entity);

                existingUser.FirstName = entity.FirstName;
                existingUser.LastName = entity.LastName;
                existingUser.Email = entity.Email;
                existingUser.PhoneNumber = entity.PhoneNumber;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(CustomerRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.CustomerId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(CustomerRepository));
                return false;
            }
        }
    }
}

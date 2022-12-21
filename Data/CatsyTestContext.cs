using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CatsyTest.Model;

namespace CatsyTest.Data
{
    public class CatsyTestContext : DbContext
    {
        public CatsyTestContext (DbContextOptions<CatsyTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; } = default!;
        public virtual DbSet<Product> Products { get; set; } = default!;
        public virtual DbSet<Order> Orders { get; set; } = default!;
        public virtual DbSet<OrderDetails> OrderDetails { get; set; } = default!;
    }
}

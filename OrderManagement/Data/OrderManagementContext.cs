using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Models
{
    public class OrderManagementContext : DbContext
    {
        public OrderManagementContext (DbContextOptions<OrderManagementContext> options)
            : base(options)
        {
        }

        public DbSet<OrderManagement.Models.Order> Order { get; set; }
    }
}

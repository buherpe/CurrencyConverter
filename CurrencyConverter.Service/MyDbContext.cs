using System;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Service
{
    public class MyDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }

    public class Request
    {
        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Json { get; set; }
    }
}
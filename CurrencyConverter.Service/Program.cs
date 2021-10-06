using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyConverter.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<MyDbContext>(options => options
                        .UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention());

                    services.AddHostedService<ExchangeRatesUpdateWorker>();
                    services.AddHostedService<TelegramBotWorker>();
                    services.AddSingleton<IClass1, Class1>();
                });
    }

    public interface IClass1
    {
        Guid Guid { get; set; }
    }

    public class Class1 : IClass1
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
    }

    public class Request
    {
        public int Id { get; set; }

        public DateTime LastUpdate { get; set; }

        public string Json { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ITNews.Data.EFContext
{
    public class NewsDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<NewsDbContext>
    {
        public NewsDbContext CreateDbContext(string[] args)
        {
            //Environment Variable
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            //Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            //DbContext Options builder
            var builder = new DbContextOptionsBuilder();

            var connectionString = configuration.GetConnectionString("NewsConnStr");

            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string cannot be empty");

            builder.UseSqlServer(connectionString);

            return new NewsDbContext(builder.Options);
        }
    }
}

using Catalog.API.Model;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace Catalog.API.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
         {
             builder
                 .AddFilter((category, level) =>
                     category == DbLoggerCategory.Database.Command.Name
                     && level == LogLevel.Information)
                 .AddConsole();
         });

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogItemsTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogTypesTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogBrandsTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=catalog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}

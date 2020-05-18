using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure
{
    public class CatalogBrandsTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .UseHiLo("catalog_brand_hilo")
                .IsRequired();
            builder.Property(sb => sb.Brand)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
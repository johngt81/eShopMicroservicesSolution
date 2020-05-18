﻿using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure
{
    public class CatalogItemsTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");
            builder.Property(ci=>ci.Id)
                .UseHiLo("catalog_hilo")
                .IsRequired();
            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(50);
            builder.Property(ci => ci.Price)
                .IsRequired(true);
            builder.HasOne(ci => ci.CatalogBrand)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogBrandId);
            builder.HasOne(ci => ci.CatalogType)
                .WithMany()
                .HasForeignKey(ci => ci.CatalogTypeId);
        }
    }
}
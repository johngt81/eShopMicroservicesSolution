using Basket.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.API.Infraestructure
{
    internal class BasketItemsTypeConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("BasketItem");
            builder.HasKey(ci =>ci.Id);
            builder.Property(ci => ci.Id)
                .UseHiLo("basket_item_hilo")
                .IsRequired();
            builder.Property(cb => cb.ProductName)
                .HasMaxLength(50)
                .IsRequired();
            //builder.HasOne(ci => ci.CustomerBasket)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.CustomerBasketId);
        }
    }
}
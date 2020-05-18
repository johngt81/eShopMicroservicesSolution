using Basket.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Infraestructure
{
    internal class CustomerBasketTypesConfiguration : IEntityTypeConfiguration<CustomerBasket>
    {
        public void Configure(EntityTypeBuilder<CustomerBasket> builder)
        {
            builder.ToTable("CustomerBasket");
            builder.HasKey(ci => ci.BuyerId);
        }
    }
}
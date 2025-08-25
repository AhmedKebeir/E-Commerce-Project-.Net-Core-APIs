using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, X => X.WithOwner()); // relation 1 to 1 total 
            builder.Property(O => O.Status)
                .HasConversion(
                    OStatus => OStatus.ToString(),//save in database
                    OStatus =>(OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)//return to database
                );

            builder.Property(O => O.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

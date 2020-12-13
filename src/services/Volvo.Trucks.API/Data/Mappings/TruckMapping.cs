using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volvo.Core.DomainObject;
using Volvo.Trucks.API.Models;

namespace Volvo.Trucks.API.Data.Mappings
{
    public class TruckMapping : IEntityTypeConfiguration<Truck>
    {
        public void Configure(EntityTypeBuilder<Truck> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ManufactureYear)
                .IsRequired()
                .HasColumnType("int");

            builder.OwnsOne(c => c.VIN, tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(Vin.VimLength)
                    .HasColumnType($"varchar({Vin.VimLength})");
            });

            builder.HasOne(c => c.Model)
            .WithMany(c => c.Trucks)
            .HasForeignKey(x=> x.ModelId);

            builder.ToTable("Truck");
        }
    }
}

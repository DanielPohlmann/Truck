using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volvo.Trucks.API.Models;

namespace Volvo.Trucks.API.Data.Mappings
{
    public class ModelMapping : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.ModelYear)
                .IsRequired()
                .HasColumnType("int");

            builder.ToTable("Model");
        }
    }
}

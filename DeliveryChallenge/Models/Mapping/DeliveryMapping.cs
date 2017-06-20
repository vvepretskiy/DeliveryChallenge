using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Mapping
{
	public class DeliveryMapping : EntityTypeConfiguration<Delivery>
	{
		public DeliveryMapping()
		{
			this.HasKey(x => x.Id)
				.Property(x => x.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(x => x.Name)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(255)
				.IsRequired();

			this.Property(t => t.DeliveryTypeId).IsRequired();

			this.HasRequired(x => x.Type).WithMany(y => y.Deliveries).HasForeignKey(z => z.DeliveryTypeId);

			this.HasMany(x => x.Details).WithOptional().HasForeignKey(x => x.DeliveryId).WillCascadeOnDelete(true);

			this.ToTable("Delivery");
		}
	}
}

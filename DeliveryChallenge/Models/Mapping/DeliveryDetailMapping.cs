using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Mapping
{
	public class DeliveryDetailMapping : EntityTypeConfiguration<DeliveryDetail>
	{
		public DeliveryDetailMapping()
		{
			this.HasKey(x => x.Id)
				.Property(x => x.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(x => x.Key)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			this.Property(x => x.Value)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(4000);

			this.Property(x => x.DeliveryId).HasColumnType("INT");

			this.ToTable("DeliveryDetail");
		}
	}
}

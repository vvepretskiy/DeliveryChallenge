using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Mapping
{
	public class DeliveryTypeMapping : EntityTypeConfiguration<DeliveryType>
	{
		public DeliveryTypeMapping()
		{
			this.HasKey(x => x.Id)
				.Property(x => x.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(x => x.Name)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			this.ToTable("DeliveryType");
		}
	}
}

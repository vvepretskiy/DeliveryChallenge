using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Mapping
{
	public class EmployeeMapping : EntityTypeConfiguration<Employee>
	{
		public EmployeeMapping()
		{
			this.HasKey(x => x.Id)
				.Property(x => x.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(x => x.FirstName)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			this.Property(x => x.SecondName)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			this.HasMany(x => x.Deliveries)
				.WithMany(y => y.Employees)
				.Map(y =>
				{
					y.MapLeftKey("EmployeeId");
					y.MapRightKey("DeliveryId");
					y.ToTable("TeamMember");
				});

			this.ToTable("Employee");
		}
	}
}

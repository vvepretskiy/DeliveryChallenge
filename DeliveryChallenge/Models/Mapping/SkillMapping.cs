using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using DeliveryChallenge.Models.Entity;

namespace DeliveryChallenge.Models.Mapping
{
	public class SkillMapping : EntityTypeConfiguration<Skill>
	{
		public SkillMapping()
		{
			this.HasKey(x => x.Id)
				.Property(x => x.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.Property(x => x.Name)
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50)
				.IsRequired()
				.HasColumnAnnotation("Index", 
					new IndexAnnotation(new[] {new IndexAttribute("Index") {IsUnique = true}}));

			this.HasMany(x => x.Employees)
				.WithMany(y => y.Skills)
				.Map(y =>
				{
					y.MapRightKey("EmployeeId");
					y.MapLeftKey("SkillId");
					y.ToTable("EmployeeSkill");
				});

			this.HasMany(x => x.Deliveries)
				.WithMany(y => y.Skills)
				.Map(y =>
				{
					y.MapRightKey("DeliveryId");
					y.MapLeftKey("SkillId");
					y.ToTable("DeliverySkill");
				});

			this.ToTable("Skill");
		}
	}
}

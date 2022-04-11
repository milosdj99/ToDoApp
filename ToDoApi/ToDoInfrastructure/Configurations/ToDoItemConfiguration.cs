using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoCore;

namespace ToDoInfrastructure.Configurations
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
    {
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content).IsRequired().HasMaxLength(200);

            builder.HasOne(item => item.ToDoList)
                .WithMany(list => list.Items)
                .HasForeignKey(i => i.ToDoListId);

            builder.Property(x => x.Owner).IsRequired();
        }
    }
}

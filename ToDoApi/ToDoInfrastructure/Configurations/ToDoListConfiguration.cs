using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoCore;

namespace ToDoInfrastructure.Configurations
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(20);

            builder.Property(x => x.Owner).IsRequired();
        }
    }
}

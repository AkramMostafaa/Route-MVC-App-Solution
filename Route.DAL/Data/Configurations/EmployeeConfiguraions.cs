using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.DAL.Data.Configurations
{
    public class EmployeeConfiguraions : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E=>E.Salary)
                .HasColumnType("decimal(18,2)");

            builder.Property(E => E.Name)
                .IsRequired(true)
                .HasMaxLength(50);


        }
    }
}

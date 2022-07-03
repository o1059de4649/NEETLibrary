using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSELibrary.Com.Attribute;
using CSELibrary.Com.Models;
using Microsoft.EntityFrameworkCore;

namespace Cse.Models.Database
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=tcp:tukimuradb.database.windows.net,1433;Initial Catalog=tukimuradev;User Id=sqldbadmin@tukimuradb;Password=f)wTAHf7CHgb");
        }

        public virtual DbSet<TEST_USER> TEST_USERs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEST_USER>(entity =>
            {
                entity.Property(e => e.ID).IsFixedLength();

                entity.Property(e => e.NAME).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    [DatabaseName("tsukimuradbdev")]
    [Table("TEST_USER")]
    public partial class TEST_USER : EfwBaseModel
    {
        [Key]
        [StringLength(10)]
        public string ID { get; set; }
        [StringLength(10)]
        public string NAME { get; set; }
    }
}
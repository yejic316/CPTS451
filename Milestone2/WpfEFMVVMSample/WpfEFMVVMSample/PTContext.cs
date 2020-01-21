namespace WpfEFMVVMSample
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PTContext : DbContext
    {
        public PTContext()
            : base("name=PTContext")
        {
        }

        public virtual DbSet<product> products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<product>()
                .Property(e => e.pdtname)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.unitprice)
                .HasPrecision(19, 4);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MoneyHeist.Models.Model
{
	public partial class MoneyHeistContext : DbContext
	{
		public MoneyHeistContext()
		{
		}

		public MoneyHeistContext(DbContextOptions<MoneyHeistContext> options)
			: base( options )
		{
		}

		public virtual DbSet<Member> Member { get; set; }
		public virtual DbSet<Skills> Skills { get; set; }
		public virtual DbSet<Heist> Heists { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if ( !optionsBuilder.IsConfigured )
			{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
				optionsBuilder.UseSqlServer( "Server=baza;Database=MoneyHeist;Trusted_Connection=True;" );
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation( "Relational:Collation", "Croatian_CI_AS" );

		

			OnModelCreatingPartial( modelBuilder );
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}

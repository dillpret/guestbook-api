using Guestbook.Entities;
using Microsoft.EntityFrameworkCore;

namespace Guestbook.DataAccess
{
	public class GuestBookContext : DbContext
	{
		public DbSet<GuestBookEntry> GuestBookEntries { get; set; }

		public GuestBookContext(DbContextOptions<GuestBookContext> options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<GuestBookEntry>(entity =>
			{
				entity.Property(e => e.Name).IsRequired();
				entity.Property(e => e.Message).IsRequired();
				entity.Property(e => e.TimeStamp).IsRequired();
			});
		}
	}
}

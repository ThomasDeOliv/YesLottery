using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Yes.Server.Datas.DbAccess.Entities;

namespace Yes.Server.Datas.DbAccess
{
    /// <summary>
    /// Custom DbContext instance
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class YesDbContext : DbContext
    {    
        public YesDbContext(DbContextOptions<YesDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Records of Draws
        /// </summary>
        public virtual DbSet<DrawEntity> Draws { get; set; }

        /// <summary>
        /// Records of Ranks
        /// </summary>
        public virtual DbSet<RankEntity> Ranks { get; set; }

        /// <summary>
        /// Records of Tickets
        /// </summary>
        public virtual DbSet<TicketEntity> Tickets { get; set; }

        /// <summary>
        /// Records of Statistics
        /// </summary>
        public virtual DbSet<StatisticEntity> Statistics { get; set; }

        /// <summary>
        /// Setting some parameters to configure relations between tables
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DrawEntity>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.DrawedNumbers)
                .HasColumnType("char")
                .HasMaxLength(17)
                .IsFixedLength(true);
            });

            builder.Entity<RankEntity>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasData(new List<RankEntity>()
                {
                    new RankEntity() { Id = 1, Descriptor = "First Rank, all 6 numbers are valid" },
                    new RankEntity() { Id = 2, Descriptor = "Second Rank, 5 of the 6 numbers are valid" },
                    new RankEntity() { Id = 3, Descriptor = "Third Rank, 4 of the 6 numbers are valid" },
                    new RankEntity() { Id = 4, Descriptor = "Fourth Rank, less than 4 numbers are valid..." },
                    new RankEntity() { Id = 5, Descriptor = "Default Rank before until the end of a draw" }
                });
            });

            builder.Entity<TicketEntity>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.AccessCode)
                .HasColumnType("char")
                .HasMaxLength(22)
                .IsFixedLength(true);

                entity.Property(t => t.PlayedNumbers)
                .HasColumnType("char")
                .HasMaxLength(17)
                .IsFixedLength(true);

                entity.HasOne(t => t.Draw)
                .WithMany(d => d.Tickets)
                .HasForeignKey(t => t.FKDrawId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Rank)
                .WithMany(r => r.Tickets)
                .HasForeignKey(t => t.FKRankId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<StatisticEntity>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.Draw)
                .WithMany(d => d.Statistics)
                .HasForeignKey(s => s.FKDrawId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Draw)
                .WithMany(d => d.Statistics)
                .HasForeignKey(s => s.FKDrawId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }
    }
}
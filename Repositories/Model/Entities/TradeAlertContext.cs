using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class TradeAlertContext : DbContext
    {
        public TradeAlertContext()
        {
        }

        public TradeAlertContext(DbContextOptions<TradeAlertContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Markets> Markets { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<QuotesAlerts> QuotesAlerts { get; set; }
        public virtual DbSet<QuotesAlertsTypes> QuotesAlertsTypes { get; set; }
        public virtual DbSet<QuotesPriority> QuotesPriority { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot _config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
                optionsBuilder.UseLazyLoadingProxies()
                    .UseSqlServer(_config.GetConnectionString("TradeAlert"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Markets>(entity =>
            {
                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.state)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.Property(e => e.currency)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.regularMarketChangePercent).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.regularMarketPrice).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.updateDate).HasColumnType("datetime");

                entity.HasOne(d => d.market)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.marketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotes_Markets");

                entity.HasOne(d => d.priority)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.priorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotes_QuotesPriority");
            });

            modelBuilder.Entity<QuotesAlerts>(entity =>
            {
                entity.Property(e => e.description)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.price).HasColumnType("decimal(12, 2)");

                entity.HasOne(d => d.QuoteAlertType)
                    .WithMany(p => p.QuotesAlerts)
                    .HasForeignKey(d => d.QuoteAlertTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotesAlerts_QuotesAlertsTypes");

                entity.HasOne(d => d.Quote)
                    .WithMany(p => p.QuotesAlerts)
                    .HasForeignKey(d => d.QuoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuotesAlerts_Quotes");
            });

            modelBuilder.Entity<QuotesAlertsTypes>(entity =>
            {
                entity.Property(e => e.description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.message)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<QuotesPriority>(entity =>
            {
                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

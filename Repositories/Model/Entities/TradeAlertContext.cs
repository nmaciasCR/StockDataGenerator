using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<Markets> Markets { get; set; }
        public virtual DbSet<Portfolio> Portfolio { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<QuotesAlerts> QuotesAlerts { get; set; }
        public virtual DbSet<QuotesAlertsTypes> QuotesAlertsTypes { get; set; }
        public virtual DbSet<QuotesPriority> QuotesPriority { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.updateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Markets>(entity =>
            {
                entity.Property(e => e.description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.flag)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.state)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.HasKey(e => e.quoteId);

                entity.Property(e => e.quoteId).ValueGeneratedNever();

                entity.HasOne(d => d.quote)
                    .WithOne(p => p.Portfolio)
                    .HasForeignKey<Portfolio>(d => d.quoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Portfolio_Quotes");
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.Property(e => e.dateReview).HasColumnType("datetime");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.regularMarketChange).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.regularMarketChangePercent).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.regularMarketPrice).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.updateDate).HasColumnType("datetime");

                entity.HasOne(d => d.currency)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.currencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Quotes_Currencies");

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

                entity.Property(e => e.regularMarketPercentDiff).HasColumnType("decimal(12, 2)");

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

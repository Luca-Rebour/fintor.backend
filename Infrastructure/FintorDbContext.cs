using Domain.Entities;
using Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class FintorDbContext: DbContext
    {
        public FintorDbContext(DbContextOptions<FintorDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<PushSubscription> PushSubscriptions => Set<PushSubscription>();
        public DbSet<Currency> Currencies => Set<Currency>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedCurrencies();

            modelBuilder.Entity<Account>(builder =>
            {
                builder.HasKey(a => a.Id);

                builder.HasOne(a => a.User)
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(a => a.Currency)
                   .WithMany()
                   .HasForeignKey(a => a.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

                builder.Property(a => a.Name)
                       .IsRequired()
                       .HasMaxLength(50);
            });

            modelBuilder.Entity<Currency>(builder =>
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Code)
                       .IsRequired()
                       .HasMaxLength(5);

            });


            modelBuilder.Entity<Category>(builder =>
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Name)
                       .IsRequired()
                       .HasMaxLength(100);

                builder.Property(c => c.Icon)
                       .IsRequired()
                       .HasMaxLength(50);

                builder.Property(c => c.UserId)
                       .IsRequired();

                builder.Property(c => c.Color)
                       .IsRequired()
                       .HasMaxLength(100);

                builder.HasOne<User>()
                       .WithMany()
                       .HasForeignKey(c => c.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(c => new { c.UserId, c.Name })
                    .IsUnique();

            });
 

            modelBuilder.Entity<Transaction>(builder =>
            {
                builder.HasKey(m => m.Id);

                builder.Property(m => m.Amount)
                    .IsRequired()
                    .HasPrecision(18, 2);

                builder.HasOne(m => m.Category)
                    .WithMany()
                    .HasForeignKey(m => m.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(m => m.Account)
                   .WithMany()
                   .HasForeignKey(m => m.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

                builder.Property(m => m.Description)
                       .IsRequired()
                       .HasMaxLength(250);

                builder.Property(m => m.Date)
                       .IsRequired();

                builder.Property(m => m.TransactionType)
                       .IsRequired();
            });

            modelBuilder.Entity<Notification>(builder =>
            {
                builder.HasKey(n => n.Id);

                builder.Property(n => n.Title)
                       .IsRequired()
                       .HasMaxLength(200);

                builder.Property(n => n.IsRead)
                       .IsRequired();

                builder.Property(n => n.CreatedAt)
                       .IsRequired();

                builder.Property(n => n.TriggerAt)
                       .IsRequired();

                builder.Property(n => n.UserId)
                       .IsRequired();

                builder.HasOne<User>()
                       .WithMany()
                       .HasForeignKey(n => n.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PushSubscription>(builder =>
            {
                builder.HasKey(ps => ps.Id);

                builder.Property(ps => ps.Provider)
                       .IsRequired();

                builder.Property(ps => ps.Platform)
                       .IsRequired();

                builder.Property(ps => ps.DeviceId)
                       .IsRequired();

                builder.Property(ps => ps.LastSeenAt)
                .IsRequired();

                builder.Property(ps => ps.UserId)
                       .IsRequired();

                builder.HasOne<User>() 
                       .WithMany()
                       .HasForeignKey(ps => ps.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecurringTransaction>(builder =>
            {
                builder.HasKey(rm => rm.Id);

                builder.Property(rm => rm.Name)
                       .IsRequired()
                       .HasMaxLength(100);

                builder.Property(rm => rm.Amount)
                    .IsRequired()
                    .HasPrecision(18, 2);


                builder.Property(rm => rm.Description)
                       .IsRequired()
                       .HasMaxLength(250);

                builder.Property(rm => rm.TransactionType)
                       .IsRequired();

                builder.Property(rm => rm.Frequency)
                       .IsRequired();

                builder.Property(rm => rm.StartDate)
                       .IsRequired();

                builder.Property(rm => rm.EndDate)
                       .IsRequired();

                builder.Property(rm => rm.LastGeneratedAt)
                       .IsRequired(false);



                builder.HasOne(rm => rm.Category)
                       .WithMany()
                       .HasForeignKey(rm => rm.CategoryId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(rm => rm.Account)
                       .WithMany()
                       .HasForeignKey(rm => rm.AccountId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasMany(rm => rm.Transactions)
                       .WithOne(m => m.RecurringTransaction)
                       .HasForeignKey(m => m.RecurringMovementId)
                       .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);

                builder.Property(u => u.Email)
                       .IsRequired()
                       .HasMaxLength(100);

                builder.HasIndex(u => u.Email)
                       .IsUnique();

                builder.Property(u => u.PasswordHash)
                       .IsRequired()
                       .HasMaxLength(256);

                builder.Property(u => u.Name)
                       .IsRequired()
                       .HasMaxLength(50);

                builder.Property(u => u.LastName)
                       .IsRequired()
                       .HasMaxLength(50);

                builder.Property(u => u.DateOfBirth)
                       .IsRequired();

                builder.Property(u => u.Provider)
                       .HasMaxLength(50);

                builder.Property(u => u.ProviderId)
                       .HasMaxLength(100);

                builder.Property(u => u.CreatedAt)
                       .IsRequired();

                builder.HasOne(rm => rm.BaseCurrency)
                   .WithMany()
                   .HasForeignKey(rm => rm.BaseCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}

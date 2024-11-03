using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.DataAccess
{
    public class LeitnerBoxDbcontext :DbContext, ILeitnerBoxDbcontext, IDesignTimeDbContextFactory<LeitnerBoxDbcontext>
    {
        protected string Schema => "LeitnerBox";
        public LeitnerBoxDbcontext(DbContextOptions<LeitnerBoxDbcontext> options) : base(options)
        {
        }
        public LeitnerBoxDbcontext()
        {
        }

        public DbSet<UserBox>? userBox { get; set; }
        public DbSet<BoxData>? Box { get; set; }
        public DbSet<dictionaryRoot>? ApiDictionaryRoot { get; set; }
        public DbSet<Phonetic>? Phonetic { get; set; }
        public DbSet<Definition>? Definition { get; set; }

        public DbSet<License>? License { get; set; }

        public DbSet<Meaning>? Meaning { get; set; }
        public DbSet<SimiliarWords> similiarWords { get; set; }
        public DbSet<SearchedWord> searchedWords { get; set; }
        public DbSet<UserBoxView>? userBoxView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBoxView>(entity =>
            {
                entity
                    
                    .ToView("UserBoxView", "LeitnerBox");

                entity.Property(e => e.EnglishWord).HasMaxLength(50);
                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .UseCollation("Arabic_CI_AS");
                entity.Property(e => e.Working).HasColumnName("working");
            });
            if (!string.IsNullOrWhiteSpace(Schema))
            {
                modelBuilder.HasDefaultSchema(Schema);
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<SimiliarWords>()
              .HasNoKey();
            modelBuilder.Entity<SearchedWord>()
            .HasNoKey();
        }

        public LeitnerBoxDbcontext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("LeitnerBox");

           



            var optionsBuilder = new DbContextOptionsBuilder<LeitnerBoxDbcontext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LeitnerBoxDbcontext(optionsBuilder.Options);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}

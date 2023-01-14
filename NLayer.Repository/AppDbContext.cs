﻿
using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }


        public override int SaveChanges()
        {


            UpdateChangeTracker();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            UpdateChangeTracker();
            return base.SaveChangesAsync(cancellationToken);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                Width = 200,
                ProductId = 1


            },
            new ProductFeature()
            {
                Id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 500,
                ProductId = 2


            }


            );




            base.OnModelCreating(modelBuilder);
        }


        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReferance)
                {

                    switch (item.State)
                    {

                        case EntityState.Added:
                            {
                                Entry(entityReferance).Property(x => x.UpdatedDate).IsModified = false;
                                entityReferance.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReferance).Property(x => x.CreatedDate).IsModified = false;
                                entityReferance.UpdatedDate = DateTime.Now;
                                break;
                            };
                    }


                }

            }
        }
    }
}
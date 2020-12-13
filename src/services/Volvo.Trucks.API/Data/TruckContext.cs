using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Core.Data;
using Volvo.Core.DomainObject;
using Volvo.Core.Mediator;
using Volvo.Core.Messages;
using Volvo.Trucks.API.Models;

namespace Volvo.Trucks.API.Data
{
    public sealed class TruckContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public TruckContext(DbContextOptions<TruckContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Cascade;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TruckContext).Assembly);

            InitialData(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediatorHandler.PublishEvents(this);

            return success;
        }


        //TODO temp
        private void InitialData(ModelBuilder modelBuilder) 
        {
            var list = new List<Model>() {
                new Model("FH", 2020){ Id = Guid.Parse("8132b9df-d14b-45ae-8b7f-9fe7ecaef8a0") },
                new Model("FM", 2020){ Id = Guid.Parse("b6e9987a-d4c5-48a6-82fd-a470a135dbfa") },
                new Model("FH", 2019){ Id = Guid.Parse("a6f734da-f964-41f7-b2c5-1f71f0baba47") },
                new Model("FM", 2019){ Id = Guid.Parse("b708de2e-85dc-418a-85e0-0fabfe2c7903") }
            };
            modelBuilder.Entity<Model>().HasData(list);
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notification != null && x.Entity.Notification.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notification)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.CleanEvent());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}

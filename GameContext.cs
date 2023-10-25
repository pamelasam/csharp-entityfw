using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Threading;

namespace ConsoleApp
{
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<UserInput> UserInputs { get; set; }
        public DbSet<AuditEntity> AuditEntities { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbConnString = ConfigurationManager.AppSettings.Get("DBConnString");
            optionsBuilder.UseSqlServer(dbConnString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuditEntity>().Property(ae => ae.Changes).HasConversion(value => JsonConvert.SerializeObject(value), serializedValue => JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedValue));
        }
        private List<AuditEntity> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntity>();
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            {
                var entityName = entry.Entity.GetType().Name;
                var entityId = entry.Properties.Single(p => p.Metadata.IsPrimaryKey()).CurrentValue.ToString();
                var action = entry.State switch
                {
                    EntityState.Added => "Created",
                    EntityState.Modified => "Modified",
                    EntityState.Deleted => "Deleted",
                    _ => "Unknown"
                };

                var auditEntry = new AuditEntity
                {
                    EntityName = entityName,
                    EntityId = entityId,
                    ActionType = action,
                    Changes = entry.Properties.Select(p => new { p.Metadata.Name, p.CurrentValue }).ToDictionary(i => i.Name, i => i.CurrentValue),
                    TimeStamp = DateTime.Now,
                    //PropertyName = propertyName,
                    //OriginalValue = originalValue,
                    //NewValue = newValue,
                    //UserId = currentUserId

                    // TempProperties are properties that are only generated on save, e.g. ID's
                    // These properties will be set correctly after the audited entity has been saved
                    TempProperties = entry.Properties.Where(p => p.IsTemporary).ToList(),
                };
                auditEntries.Add(auditEntry);
            }
            return auditEntries;
        }
        private void OnAfterSaveChanges(List<AuditEntity> auditEntries)
        {
            if (!(auditEntries == null || auditEntries.Count == 0))
            {
                // For each temporary property in each audit entry - update the value in the audit entry to the actual (generated) value
                foreach (var entry in auditEntries)
                {
                    foreach (var prop in entry.TempProperties)
                    {
                        if (prop.Metadata.IsPrimaryKey())
                        {
                            entry.EntityId = prop.CurrentValue.ToString();
                            entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                        }
                        else
                        {
                            entry.Changes[prop.Metadata.Name] = prop.CurrentValue;
                        }
                    }
                }
                AuditEntities.AddRange(auditEntries);
            }
            base.SaveChanges();
        }
        public override int SaveChanges()
        {
            // Get audit entries
            var auditEntries = OnBeforeSaveChanges();
            var result = base.SaveChanges();
            // Save audit entries
            OnAfterSaveChanges(auditEntries);
            return result;
        }
    }
}

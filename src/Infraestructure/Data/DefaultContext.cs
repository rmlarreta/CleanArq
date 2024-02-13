using Infraestructure.Entities.Common;
using Infraestructure.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Data
{
    public class DefaultContext(DbContextOptions<DefaultContext> options, IContextProvider contextProvider) : DbContext(options)
    {
        public static readonly ILoggerFactory _dbLoggerFactory = new LoggerFactory(new[] {
                                                                                    new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                                                                                 });
        private readonly IContextProvider _contextProvider = contextProvider;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseLoggerFactory(_dbLoggerFactory)
                .EnableSensitiveDataLogging();
#endif
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            HandleAuditableProperties();

            try
            {
                // perform database operations using the context instance
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                // handle database errors
                if (sqlEx.Number == 4060 || sqlEx.Number == 10928)
                {
                    // Retry the operation after 5 seconds
                    Thread.Sleep(10000);
                    return base.SaveChanges(acceptAllChangesOnSuccess);
                }
                else
                {
                    throw;
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleAuditableProperties();

            try
            {
                // perform database operations using the context instance
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
            {
                // handle database errors
                if (sqlEx.Number == 4060 || sqlEx.Number == 10928)
                {
                    // Retry the operation after 5 seconds
                    Thread.Sleep(10000);
                    return await base.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureMapping(modelBuilder);

            Seeds.SeedData.Seed(modelBuilder);

        }

        protected void HandleAuditableProperties()
        {
            var userName = _contextProvider.Username;
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State != EntityState.Detached || e.State != EntityState.Unchanged))
            {
                if (entry.Entity is Auditable auditableEntity)
                {

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditableEntity.CreatedDate = DateTime.UtcNow;
                            auditableEntity.CreatedBy = userName ?? auditableEntity.CreatedBy;
                            break;
                        case EntityState.Modified:
                            auditableEntity.ModifiedDate = DateTime.UtcNow;
                            auditableEntity.ModifiedBy = userName ?? auditableEntity.CreatedBy;
                            break;
                        case EntityState.Deleted:
                            auditableEntity.DeletedDate = DateTime.UtcNow;
                            auditableEntity.ModifiedDate = DateTime.UtcNow;
                            auditableEntity.ModifiedBy = userName ?? auditableEntity.CreatedBy;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
        }

        private static void ConfigureMapping(ModelBuilder modelBuilder)
        {
            //  modelBuilder 
            // .ApplyConfiguration(new LogConfiguration();
        }
    }
}

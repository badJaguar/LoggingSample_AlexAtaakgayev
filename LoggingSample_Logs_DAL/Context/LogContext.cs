using System.Data.Entity;
using LoggingSample_Logs_DAL.Entities;

namespace LoggingSample_Logs_DAL.Context
{
    public class LogContext : DbContext
    {
        public LogContext() : base("LoggingSampleLogs")
        {
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<LogContext, Migrations.Configuration>());
        }

        public DbSet<LogMessage> LogMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
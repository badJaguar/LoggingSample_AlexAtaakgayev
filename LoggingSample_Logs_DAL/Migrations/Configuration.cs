namespace LoggingSample_Logs_DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LoggingSample_Logs_DAL.Context.LogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LoggingSample_Logs_DAL.Context.LogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //context.LogMessages.AddOrUpdate();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}

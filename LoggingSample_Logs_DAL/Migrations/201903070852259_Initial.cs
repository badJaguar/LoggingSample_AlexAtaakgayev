namespace LoggingSample_Logs_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineName = c.String(),
                        LoggerName = c.String(),
                        Message = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Level = c.String(),
                        MessageSource = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogMessages");
        }
    }
}

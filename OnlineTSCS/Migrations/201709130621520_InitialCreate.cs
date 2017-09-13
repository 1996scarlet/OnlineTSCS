namespace OnlineTSCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountName = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 32),
                        Type = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        School = c.String(maxLength: 16),
                        Mobile = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionName = c.String(),
                        ControllerName = c.String(),
                        ActionParameters = c.String(),
                        AccessDate = c.DateTime(nullable: false),
                        ActionAccount = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogModels");
            DropTable("dbo.AccountModels");
        }
    }
}

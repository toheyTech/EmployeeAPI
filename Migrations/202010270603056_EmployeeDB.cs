namespace EmployeeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Nationality = c.String(),
                        BirthDate = c.DateTime(),
                        Gender = c.String(),
                        Active = c.Boolean(nullable: false),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Code, cascadeDelete: true)
                .Index(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Code", "dbo.People");
            DropIndex("dbo.Projects", new[] { "Code" });
            DropTable("dbo.Projects");
            DropTable("dbo.People");
        }
    }
}

namespace Partie_Api_Amd_Logique_Metier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Prenom = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 100),
                        RoleId = c.Int(nullable: false),
                        Specialite = c.String(maxLength: 100),
                        Biographie = c.String(maxLength: 1000),
                        LienLinkedIn = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Formations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titre = c.String(nullable: false, maxLength: 100),
                        CategoryId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        FormateurId = c.Int(nullable: false),
                        Prix = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimationDeDuree = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Users", t => t.FormateurId)
                .Index(t => t.CategoryId)
                .Index(t => t.FormateurId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titre = c.String(nullable: false, maxLength: 100),
                        DelivranceDate = c.DateTime(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        FormationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formations", t => t.FormationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ParticipantId, cascadeDelete: true)
                .Index(t => t.ParticipantId)
                .Index(t => t.FormationId);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Feedback = c.String(maxLength: 1000),
                        ParticipantId = c.Int(nullable: false),
                        FormationId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formations", t => t.FormationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ParticipantId, cascadeDelete: true)
                .Index(t => t.ParticipantId)
                .Index(t => t.FormationId);
            
            CreateTable(
                "dbo.Inscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParticipaantId = c.Int(nullable: false),
                        FormationId = c.Int(nullable: false),
                        DateInscription = c.DateTime(nullable: false),
                        Statut = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ParticipaantId, cascadeDelete: true)
                .ForeignKey("dbo.Formations", t => t.FormationId, cascadeDelete: true)
                .Index(t => t.ParticipaantId)
                .Index(t => t.FormationId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        FormationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.Formations", t => t.FormationId, cascadeDelete: true)
                .Index(t => t.ParticipantId)
                .Index(t => t.FormationId);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Type = c.String(nullable: false, maxLength: 50),
                        Url = c.String(nullable: false),
                        FormationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Formations", t => t.FormationId, cascadeDelete: true)
                .Index(t => t.FormationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "FormationId", "dbo.Formations");
            DropForeignKey("dbo.Media", "FormationId", "dbo.Formations");
            DropForeignKey("dbo.Inscriptions", "FormationId", "dbo.Formations");
            DropForeignKey("dbo.Formations", "FormateurId", "dbo.Users");
            DropForeignKey("dbo.Certificates", "ParticipantId", "dbo.Users");
            DropForeignKey("dbo.Payments", "ParticipantId", "dbo.Users");
            DropForeignKey("dbo.Inscriptions", "ParticipaantId", "dbo.Users");
            DropForeignKey("dbo.Evaluations", "ParticipantId", "dbo.Users");
            DropForeignKey("dbo.Evaluations", "FormationId", "dbo.Formations");
            DropForeignKey("dbo.Certificates", "FormationId", "dbo.Formations");
            DropForeignKey("dbo.Formations", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.Media", new[] { "FormationId" });
            DropIndex("dbo.Payments", new[] { "FormationId" });
            DropIndex("dbo.Payments", new[] { "ParticipantId" });
            DropIndex("dbo.Inscriptions", new[] { "FormationId" });
            DropIndex("dbo.Inscriptions", new[] { "ParticipaantId" });
            DropIndex("dbo.Evaluations", new[] { "FormationId" });
            DropIndex("dbo.Evaluations", new[] { "ParticipantId" });
            DropIndex("dbo.Certificates", new[] { "FormationId" });
            DropIndex("dbo.Certificates", new[] { "ParticipantId" });
            DropIndex("dbo.Formations", new[] { "FormateurId" });
            DropIndex("dbo.Formations", new[] { "CategoryId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropTable("dbo.Media");
            DropTable("dbo.Payments");
            DropTable("dbo.Inscriptions");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Certificates");
            DropTable("dbo.Categories");
            DropTable("dbo.Formations");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}

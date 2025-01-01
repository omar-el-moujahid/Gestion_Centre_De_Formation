namespace Partie_Api_Amd_Logique_Metier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajoutercolonnedesequencedeurlmediaetformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formations", "url_image", c => c.String(nullable: false));
            AddColumn("dbo.Media", "nombredesequence", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Media", "nombredesequence");
            DropColumn("dbo.Formations", "url_image");
        }
    }
}

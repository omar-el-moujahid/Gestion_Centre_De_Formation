//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Partie_Consumation_API_Frontend.Areas.Identity.Data;

//namespace Partie_Consumation_API_Frontend.Areas.Identity.Data;

//public class Partie_Consumation_API_FrontendContextSample : IdentityDbContext<SampleUser>
//{
//    public Partie_Consumation_API_FrontendContextSample(DbContextOptions<Partie_Consumation_API_FrontendContextSample> options)
//        : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        base.OnModelCreating(builder);
//        // Customize the ASP.NET Identity model and override the defaults if needed.
//        // For example, you can rename the ASP.NET Identity table names and more.
//        // Add your customizations after calling base.OnModelCreating(builder);

//        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
//    }
//}


//public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<SampleUser>
//{
//    public void Configure(EntityTypeBuilder<SampleUser> builder)
//    {
//        builder.Property(x => x.FirstName).HasMaxLength(180);
//        builder.Property(x => x.LastName).HasMaxLength(180);
//    }
//}
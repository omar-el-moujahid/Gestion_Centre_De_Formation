using Partie_Consumation_API_Frontend.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<FormationService>();

builder.Services.AddSingleton<FormateurService>();
builder.Services.AddHttpClient<FormateurService>();

builder.Services.AddHttpClient<AuthService>();
builder.Services.AddHttpClient<PaiementService>();
builder.Services.AddHttpClient<AdmineService>();

/**********************************************************/


builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromDays(1);
});



/***********************************************************/



builder.Services.AddHttpClient<InscriptionService>();
builder.Services.AddHttpClient<ParticipantService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Temps d'inactivité avant expiration
    options.Cookie.HttpOnly = true; // Sécuriser les cookies de session
    options.Cookie.IsEssential = true; // Nécessaire pour la conformité RGPD
});
var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

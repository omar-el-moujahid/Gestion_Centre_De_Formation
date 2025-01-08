using Partie_Consumation_API_Frontend.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Partie_Consumation_API_Frontend.Model;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Partie_Consumation_API_FrontendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Partie_Consumation_API_FrontendContext") ?? throw new InvalidOperationException("Connection string 'Partie_Consumation_API_FrontendContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<FormationService>();

builder.Services.AddSingleton<FormateurService>();
builder.Services.AddHttpClient<FormateurService>();

builder.Services.AddHttpClient<AuthService>();
builder.Services.AddHttpClient<PaiementService>();
builder.Services.AddHttpClient<AdmineService>();

builder.Services.AddSingleton<ProfileService>();

builder.Services.AddHttpClient<AuthService>();
builder.Services.AddHttpClient<PaiementService>();

builder.Services.AddTransient<ProfileService>();
builder.Services.AddHttpClient<ProfileService>();

builder.Services.AddHttpClient<ProfileService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/login";
        options.AccessDeniedPath = "/Auth/login";
    });

builder.Services.AddControllersWithViews();





/**********************************************************/


builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromDays(1);
});


builder.Services.AddDbContext<Partie_Consumation_API_FrontendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


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

// Register EvaluationService
builder.Services.AddScoped<EvaluationService>();


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

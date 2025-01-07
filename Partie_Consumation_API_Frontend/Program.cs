using Microsoft.AspNetCore.Authentication.Cookies;
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

builder.Services.AddHttpClient<FormateurService>();
builder.Services.AddTransient<ProfileService>();


builder.Services.AddHttpClient<FormateurService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/login";
        options.AccessDeniedPath = "/Auth/login";
    });



builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");






// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

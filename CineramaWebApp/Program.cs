using CineramaWebApp.Repositories;
using CineramaWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Registro de Repositorios Dapper por dominio
builder.Services.AddScoped<ICineRepository, CineRepository>();
builder.Services.AddScoped<ICarteleraRepository, CarteleraRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();

builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IBoletoService, BoletoService>();
builder.Services.AddScoped<IFidelizacionService, FidelizacionService>();
// Registrar Servicio de Autenticación
builder.Services.AddScoped<IAuthService, AuthService>();
// Configurar Autenticación por Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication(); //ve usuario
app.UseAuthorization(); //ve permisos

app.UseStaticFiles();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.IOC;
using SistemaVenta.AplicacionWeb.Utilidades.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add cadena de conexion string
builder.Services.AddSingleton(new PolarisServerStringContext(builder.Configuration.GetConnectionString("CadenaSQL")));

//Login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20); //tiempo expiraci�n
    });


builder.Services.InyectarDependencia(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DashBoard}/{action=Index}/{id?}");

app.Run();
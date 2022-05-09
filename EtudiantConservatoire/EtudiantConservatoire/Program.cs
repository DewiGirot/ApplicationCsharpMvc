using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EtudiantConservatoire.Data;
using EtudiantConservatoire;
using EtudiantConservatoire.Models;
using Microsoft.AspNetCore.Builder;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EtudiantConservatoireContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EtudiantConservatoireContext") ?? throw new InvalidOperationException("Connection string 'EtudiantConservatoireContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

// https://docs.microsoft.com/fr-fr/aspnet/core/fundamentals/error-handling?view=aspnetcore-6.0
// Permet de gérer l'exception d'error 404 page not found
app.UseStatusCodePages("text/html", "<h1>Status code page</h1> <h2>Status Code: {0}</h2> <img src='https://png.pngtree.com/png-vector/20190412/ourlarge/pngtree-error-illustration-modern-flat-design-concept-of-web-page-design-png-image_930170.jpg' alt='Error'/>");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
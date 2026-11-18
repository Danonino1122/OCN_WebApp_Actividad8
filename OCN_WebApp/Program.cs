using Microsoft.EntityFrameworkCore;
using OCN_WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar OcnDbContext: le damos la "dirección" de la base (cadena OcnConnection de appsettings.json)
// y le decimos que use SQL Server.
builder.Services.AddDbContext<OcnDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OcnConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

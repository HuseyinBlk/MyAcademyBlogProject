using Blogy.Business.Extensions;
using Blogy.DataAccess.Extensions;
using Blogy.Entity.Entities;
using Blogy.WebUI.Filters;
using Blogy.WebUI.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);

builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ValidationExceptionFilter>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name : "areas",
    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Default}/{action=Index}/{id?}")
    .WithStaticAssets();

await IdentityDataSeeder.SeedAsync(app.Services);

app.Run();

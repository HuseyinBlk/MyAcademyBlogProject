using Blogy.Business.Extensions;
using Blogy.DataAccess.Extensions;
using Blogy.Entity.Entities;
using Blogy.WebUI.Filters;
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

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var users = await userManager.Users.ToListAsync();

    const string defaultPassword = "Password12*";

    foreach (var user in users)
    {
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, defaultPassword);
        await userManager.UpdateSecurityStampAsync(user);
        await userManager.UpdateAsync(user);
    }
}

app.Run();

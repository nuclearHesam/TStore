using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TStoreContext>(options =>
    options.UseSqlite("Data Source=site.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TStoreContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; 
    options.LogoutPath = "/Account/Logout"; 
    options.ExpireTimeSpan = TimeSpan.FromDays(30); 
    options.SlidingExpiration = true;
});


builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapDefaultControllerRoute();

app.Run();

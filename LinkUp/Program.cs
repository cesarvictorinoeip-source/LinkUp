using LinkUp.Infrastructure.Persistence;
using LinkUp.Infrastructure.Identity;
using LinkUp.Infrastructure.Shared;
using LinkUp.Core.Application;
using LinkUp.Infrastructure.Persistence.Contexts;
using LinkUp.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using InvestmentApp.Infrastructure.Shared;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedLayerIoc(builder.Configuration); // ✅ Agregar esta línea

// ✅ Identity con ApplicationDbContext registrado aquí
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

builder.Services.AddIdentityLayerIocForWebApp(builder.Configuration);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

await app.Services.RunIdentitySeedAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
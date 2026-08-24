using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DB;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Users;
using OnlineShop.DB.Storages;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Interfaces;
using OOnlineShop.DB.Storages;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("OnlineShopConnection");

Log.Logger = new LoggerConfiguration()
        .CreateLogger();

try
{
    Log.Information("Starting server...");

    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddControllersWithViews();

    builder.Services.AddTransient<IProductsStorage, ProductsDBStorage>();
    builder.Services.AddTransient<IServicesStorage, ServicesDBStorage>();
    builder.Services.AddTransient<IBidsStorage, BidsDBStorage>();
    builder.Services.AddTransient<ICatalogServicesStorage, CatalogServicesDBStorage>();
    builder.Services.AddTransient<IFavoritesStorage, FavoritesDBStorage>();
    builder.Services.AddTransient<IComparisonsStorage, ComparisonsDBStorage>();
    builder.Services.AddTransient<IOrdersStorage, OrdersDBStorage>();
    builder.Services.AddTransient<IUserContextService, UserContextService>();
    builder.Services.AddSingleton<IFileProvider, FileProvider>();

    builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.LoginPath = "/Account/Authorization";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie = new CookieBuilder
        {
            IsEssential = true
        };
    });

    builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        context.Database.Migrate();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        IdentityInitializer.Initialize(userManager, roleManager);
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<AnonymousIdMiddleware>();

    app.MapStaticAssets();

    app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

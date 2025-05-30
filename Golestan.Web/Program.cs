using System.Globalization;
using Golestan.Domain.Entities;
using Golestan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration Setup
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// 3. MVC Services
builder.Services.AddControllersWithViews();

// 4. Database Context (EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GolestanDB")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// 7. Authentication & Authorization
builder.Services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie();


// 10. Localization (Persian support)
// builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
//
// builder.Services.Configure<RequestLocalizationOptions>(options => {
//     var supportedCultures = new[] { new CultureInfo("fa-IR"), new CultureInfo("en-US") };
//     options.DefaultRequestCulture = new RequestCulture("fa-IR");
//     options.SupportedCultures = supportedCultures;
//     options.SupportedUICultures = supportedCultures;
// });

var app = builder.Build();

// ========== MIDDLEWARE PIPELINE ========== //

using (var scope = app.Services.CreateAsyncScope()){
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}


// 1. Exception Handling
if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
}
else{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// 3. Static Files
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true// For Persian font files
});

// 4. Routing
app.UseRouting();


// 6. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();


// 9. Endpoints
app.MapControllerRoute(
"default",
"{controller=Admin}/{action=Index}/{id?}");


app.Run();

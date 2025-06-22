using Golestan.Application.Interfaces;
using Golestan.Application.Services;
using Golestan.Domain.Entities;
using Golestan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration Setup
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// 3. MVC Services
builder.Services.AddControllersWithViews()
    .AddViewOptions(options => {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

// 4. Database Context (EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GolestanDB")));

// 5. Services 
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITermService, TermService>();
builder.Services.AddScoped<ISelectionService, SelectionService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
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

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Authentication/Login";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
});

builder.Services.AddAuthorization();


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
"{controller=Students}/{action=StudentDashboard}/{id?}");


app.Run();

using Golestan.Application.Interfaces;
using Golestan.Application.RepositoryInterfaces;
using Golestan.Application.Services;
using Golestan.Domain.Entities;
using Golestan.Infrastructure.Data.Repositories;
using Golestan.Infrastructure.Persistence;
using Golestan.Shared.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

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
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddSingleton<IEmailService, EmailService>();


// Repositories 
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<ITermRepository, TermRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ISelectionRepository, SelectionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();


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
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
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
using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;

    try{
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.SeedRootAdmin(userManager, roleManager);
    }
    catch (Exception ex){
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
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
"{controller=Account}/{action=RedirectToRoleBasedPage}/{id?}");


app.Run();

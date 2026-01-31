using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using SENSEI.BLL.AdminPortalService;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService;
using SENSEI.BLL.SystemService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection();

var configuration = builder.Configuration;

#region AdminPortalServices
builder.Services.AddSingleton<ICourseService, CourseServiceImpl>();
builder.Services.AddSingleton<ILessonService, LessonServiceImpl>();
#endregion

#region Login with Google

var googleClientId = configuration["Authentication:Google:ClientId"];
var googleClientSecret = configuration["Authentication:Google:ClientSecret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/SenseiJapaneseSchool/Login";
})
.AddGoogle(options =>
{
    options.ClientId = googleClientId;
    options.ClientSecret = googleClientSecret;
});

#endregion

#region System Services

var connectionString = configuration.GetConnectionString("dev");

builder.Services.AddSingleton<IDatabaseService>(provider =>
{
    var dbService = new DatabaseServiceImpl();
    dbService.SetConnectionString(connectionString);
    return dbService;
});

builder.Services.AddSingleton<IMailService, MailServiceImpl>();
builder.Services.AddSingleton<ISmsService, SmsServiceImpl>();
builder.Services.AddDataProtection();
builder.Services.AddSession();

#endregion

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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// AREA ROUTES FIRST
app.MapAreaControllerRoute(
    name: "AdminPortal",
    areaName: "AdminPortal",
    pattern: "AdminPortal/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "StudentPortal",
    areaName: "StudentPortal",
    pattern: "StudentPortal/{controller=Home}/{action=Index}/{id?}");

// DEFAULT ROUTE LAST
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SenseiJapaneseSchool}/{action=Index}/{id?}");


app.Run();

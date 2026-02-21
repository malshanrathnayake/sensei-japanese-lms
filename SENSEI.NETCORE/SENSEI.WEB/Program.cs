using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.SignalR;
using SENSEI.BLL.AdminPortalService;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.SignalR;
using SENSEI.SignalR.Interface;
using SENSEI.WEB.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var configuration = builder.Configuration;

#region AdminPortalServices
builder.Services.AddSingleton<ICourseService, CourseServiceImpl>();
builder.Services.AddSingleton<ILessonService, LessonServiceImpl>();
builder.Services.AddSingleton<IUserNotificationService, UserNotificationServiceImpl>();
builder.Services.AddSingleton<IBatchService, BatchServiceImpl>();
builder.Services.AddSingleton<IBatchLessonService, BatchLessonServiceImpl>();
builder.Services.AddSingleton<ILocationService, LocationServiceImpl>();
builder.Services.AddSingleton<IStudentRegistrationService, StudentRegistrationServiceImpl>();
builder.Services.AddSingleton<IUserService, UserServiceImpl>();
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
    options.AccessDeniedPath = "/SenseiJapaneseSchool/AccessDenied";
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

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", p => p.RequireRole("Manager"));
    options.AddPolicy("StudentOnly", p => p.RequireRole("Student"));
});


#endregion

#region SignalR Services
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddSingleton<IRealtimeNotifier, RealtimeNotifierImpl>();
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

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notifications");


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

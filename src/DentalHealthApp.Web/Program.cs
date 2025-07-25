using DentalHealthApp.Data.Context;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Business.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework with SQLite
builder.Services.AddDbContext<DentalHealthDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure antiforgery for CSRF protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

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

// Add session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DentalHealthDbContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        // Log exception - in production, use proper logging
        Console.WriteLine($"Database initialization failed: {ex.Message}");
    }
}

app.Run();

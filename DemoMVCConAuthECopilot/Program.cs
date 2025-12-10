using DemoMVCConAuthECopilot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// NorthwindDbContext e repository (DemoMVC.Data)
var northwindConnectionString = builder.Configuration.GetConnectionString("NorthwindConnection") ?? throw new InvalidOperationException("Connection string 'Northwind' not found.");
builder.Services.AddDbContext<DemoMVC.Data.NorthwindDbContext>(options =>
    options.UseSqlServer(northwindConnectionString));
builder.Services.AddScoped<DemoMVC.Data.Repositories.ICustomerRepository, DemoMVC.Data.Repositories.CustomerRepository>();
builder.Services.AddScoped<DemoMVC.Data.Repositories.IOrderRepository, DemoMVC.Data.Repositories.OrderRepository>();
builder.Services.AddScoped<DemoMVC.Data.Repositories.IProductRepository, DemoMVC.Data.Repositories.ProductRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Consente l'iniezione di IHttpContextAccessor nelle view
builder.Services.AddHttpContextAccessor();

// Policy per utenti con claim AutoAziendale
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AutoAziendale", policy =>
        policy.RequireClaim("AutoAziendale"));
    // Policy per visualizzazione customer solo admin
    options.AddPolicy("ViewCustomers", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();

# GitHub Copilot Instructions - ASP.NET Core MVC Project

## Project Context
This is an ASP.NET Core MVC application following Clean Architecture principles and modern .NET development practices.

## Architecture & Structure

### Layering
- **Presentation Layer**: MVC Controllers, Views, ViewModels, Tag Helpers
- **Application Layer**: Services, DTOs, Mapping Profiles, Validation
- **Domain Layer**: Entities, Value Objects, Domain Services, Interfaces
- **Infrastructure Layer**: Data Access, External Services, Repositories

### Naming Conventions
- Controllers: `{Entity}Controller` (e.g., `ProductsController`)
- Services: `I{Entity}Service` / `{Entity}Service`
- Repositories: `I{Entity}Repository` / `{Entity}Repository`
- ViewModels: `{Entity}{Action}ViewModel` (e.g., `ProductCreateViewModel`)
- DTOs: `{Entity}Dto` (e.g., `ProductDto`)

## Code Style & Patterns

### General Guidelines
- Use C# 12+ features (primary constructors, collection expressions, pattern matching)
- Prefer dependency injection over static classes
- Follow SOLID principles
- Use async/await consistently
- Implement proper exception handling with custom exception types
- Use nullable reference types (#nullable enable)

### Controller Patterns
```csharp
[Route("[controller]")]
public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync(cancellationToken);
        return View(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return NotFound();
        
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        await productService.CreateAsync(model, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
```

### Service Layer Patterns
```csharp
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(ProductCreateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ProductUpdateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public class ProductService(IProductRepository repository, IMapper mapper, ILogger<ProductService> logger) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<ProductDto>>(products);
    }
    
    // Additional methods...
}
```

### Repository Patterns
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

## Data Access (Entity Framework Core)

### DbContext Configuration
- Use fluent API for entity configuration
- Separate configurations in `IEntityTypeConfiguration<T>` classes
- Enable global query filters for soft deletes
- Configure cascade delete behavior explicitly

```csharp
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasPrecision(18, 2);
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
```

## Validation & Error Handling

### Model Validation
- Use Data Annotations for simple validation
- Implement FluentValidation for complex scenarios
- Create custom validation attributes when needed
- Always validate in both client and server side

### Exception Handling
```csharp
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred");

        var problemDetails = exception switch
        {
            ValidationException => CreateValidationProblemDetails(httpContext, exception),
            NotFoundException => CreateNotFoundProblemDetails(httpContext, exception),
            _ => CreateServerErrorProblemDetails(httpContext, exception)
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}
```

## Configuration & Dependency Injection

### Program.cs Setup
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

## Security Best Practices

- Always use `[ValidateAntiForgeryToken]` for POST actions
- Implement proper authorization with `[Authorize]` attributes
- Use role-based or policy-based authorization
- Validate and sanitize user inputs
- Use parameterized queries (EF Core does this by default)
- Implement CORS policies when needed
- Use HTTPS in production
- Store sensitive data in user secrets or Azure Key Vault

## Logging

Use structured logging with dependency injection:

```csharp
logger.LogInformation("Processing product creation for {ProductName}", productName);
logger.LogWarning("Product {ProductId} not found", id);
logger.LogError(ex, "Error creating product {ProductName}", productName);
```

## Testing Expectations

- Write unit tests for services and business logic
- Use xUnit as testing framework
- Mock dependencies with Moq or NSubstitute
- Follow AAA pattern (Arrange, Act, Assert)
- Test both success and failure scenarios
- Use `FluentAssertions` for readable assertions

## Performance Considerations

- Use async/await for I/O operations
- Implement pagination for large datasets
- Use `.AsNoTracking()` for read-only queries
- Cache frequently accessed data
- Use Select projections to load only needed data
- Implement proper indexing in database

## View Development

- Use strongly-typed views with ViewModels
- Implement Tag Helpers for reusable components
- Use partial views for repeated UI sections
- Implement client-side validation with jQuery Validation
- Use ViewData/ViewBag sparingly, prefer ViewModels
- Keep views clean, move logic to ViewModels or Tag Helpers

## Additional Notes

- Always use `CancellationToken` in async methods
- Prefer records for DTOs and ViewModels
- Use `IOptions<T>` for configuration injection
- Implement health checks for production readiness
- Use Serilog for structured logging in production
- Document complex business logic with XML comments
- Follow RESTful conventions for API-like controllers

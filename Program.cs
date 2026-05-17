using Microsoft.EntityFrameworkCore;
using Enterprises.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// DATABASE
// ==========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ==========================================
// REPOSITORIES
// ==========================================

builder.Services.AddScoped<
    Enterprises.Repositories.ICustomerRepository,
    Enterprises.Repositories.CustomerRepository>();

builder.Services.AddScoped<
    Enterprises.Repositories.AccountRepository>();

builder.Services.AddScoped<
    Enterprises.Repositories.ItemRepository>();

builder.Services.AddScoped<
    Enterprises.Repositories.OrderRepository>();

// ==========================================
// SERVICES
// ==========================================

builder.Services.AddScoped<
    Enterprises.Services.ICustomerService,
    Enterprises.Services.CustomerService>();

builder.Services.AddScoped<
    Enterprises.Services.IAccountService,
    Enterprises.Services.AccountService>();

builder.Services.AddScoped<
    Enterprises.Services.IItemService,
    Enterprises.Services.ItemService>();

builder.Services.AddScoped<
    Enterprises.Services.IOrderService,
    Enterprises.Services.OrderService>();

// ==========================================
// CONTROLLERS
// ==========================================

builder.Services.AddControllers();

// ==========================================
// CORS
// ==========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// ==========================================
// SWAGGER
// ==========================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// HTTP PIPELINE
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

// ==========================================
// DATABASE MIGRATION + SEED
// ==========================================

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context =
                services.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            SeedData.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger =
                services.GetRequiredService<ILogger<Program>>();

            logger.LogError(
                ex,
                "An error occurred while migrating or seeding the database."
            );

            throw;
        }
    }
}

app.Run();
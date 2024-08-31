using Microsoft.EntityFrameworkCore;
using RestaurantManagerAPI.Data;
using RestaurantManagerAPI.Services;
using RestaurantManagerAPI.Repositories; // Adjust the namespace to match your actual repository location
using Microsoft.OpenApi.Models;
using RestaurantManagerAPI.Application.Services;
using RestaurantManagerAPI.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQLite
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services with the DI container
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Register repositories with the DI container
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>(); // Assuming you have these repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // Assuming you have these repositories

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Allow requests from the Angular app
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "RestaurantManagerAPI", 
        Version = "v1",
        Description = "API for managing restaurant operations, including products, menu items, and orders."
    });

    // Optionally add more Swagger configuration here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Useful for showing detailed errors in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantManagerAPI v1");
        c.RoutePrefix = string.Empty; // This will make Swagger UI available at the app's root
    });
}

app.UseHttpsRedirection();

// Enable CORS middleware
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

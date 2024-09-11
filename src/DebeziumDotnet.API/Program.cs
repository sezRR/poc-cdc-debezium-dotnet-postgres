using DebeziumDotnet.API.Contexts;
using DebeziumDotnet.API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DebeziumDotnetDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("-- Development environment --".ToUpper());
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
} else {
    // Use HTTPS redirection in production if required
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

app.MapPost("/products", async (DebeziumDotnetDbContext dbContext, CreateProduct createProduct) =>
{
    var newProduct = new Product
    {
        Name = createProduct.Name,
        Description = createProduct.Description,
        Price = createProduct.Price,
        Stock = createProduct.Stock,
        CreatedAt = DateTime.UtcNow,
    };
    
    dbContext.Products.Add(newProduct);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/products/{newProduct.Id}", newProduct);
});

app.MapGet("/products", async (DebeziumDotnetDbContext dbContext) =>
{
    var products = await dbContext.Products.ToListAsync();
    return Results.Ok(products);
});

app.Run();
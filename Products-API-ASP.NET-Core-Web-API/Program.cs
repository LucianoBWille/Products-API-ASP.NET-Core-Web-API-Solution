using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Products_API_ASP.NET_Core_Web_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var products = new List<Product>
{
    new Product { ProductID = 1, Nome = "Banana", Preco = 4.6f, Quantidade = 7 },
    new Product { ProductID = 2, Nome = "Pera", Preco = 4.6f, Quantidade = 7 },
    new Product { ProductID = 3, Nome = "Maçã", Preco = 4.6f, Quantidade = 7 },
    new Product { ProductID = 4, Nome = "Melão", Preco = 4.6f, Quantidade = 7 }
};

builder.Services.AddSingleton(products);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapGet("/products", () =>
{
    var productService = app.Services.GetRequiredService<List<Product>>();
    return Results.Ok(productService);
});

app.MapGet("/products/{id}", (int id, HttpRequest request) =>
{
    var productService = app.Services.GetRequiredService<List<Product>>();

    var product = productService.FirstOrDefault(p => p.ProductID == id);

    if(product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

app.MapPost("/products", (Product product) =>
{
    var productService = app.Services.GetRequiredService<List<Product>>();

    product.ProductID = productService.Max(p => p.ProductID) + 1;

    productService.Add(product);

    return Results.Created($"/products/{product.ProductID}", product);
});

app.MapPut("/products/{id}", (int id, Product product) =>
{
    var productService = app.Services.GetRequiredService<List<Product>>();

    var existingProduct = productService.FirstOrDefault(p => p.ProductID == id);

    if (existingProduct == null)
    {
        return Results.NotFound();
    }

    existingProduct.Nome = product.Nome;
    existingProduct.Preco = product.Preco;
    existingProduct.Quantidade = product.Quantidade;

    return Results.NoContent();
});

app.MapDelete("/products/{id}", (int id) =>
{
    var productService = app.Services.GetRequiredService<List<Product>>();

    var existingProduct = productService.FirstOrDefault(p => p.ProductID == id);

    if (existingProduct == null)
    {
        return Results.NotFound();
    }

    productService.Remove(existingProduct);

    return Results.NoContent();
});

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();

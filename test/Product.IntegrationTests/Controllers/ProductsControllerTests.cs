using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Products.CreateProduct;
using Product.IntegrationTests;
using Xunit;

namespace Product.IntegrationTest.Configurations;

[Trait("Category", "SmokeTest")]
[Collection(nameof(ProductCollectionFixture))]
public class ProductsControllerTests(
        ProductContainerFixture containerFixture,
        WebApplicationFactory<Program> factory) 
    : BaseContainerTest<Program>(containerFixture, factory)
{
    private async Task<Domain.Entities.Product> InitDataAsync()
    {
        var context = ServiceProvider!.GetRequiredService<Product.Infrastructure.Database.ProductDbContext>();
        var entityEntry = await context.Products.AddAsync(new Product.Domain.Entities.Product
        {
            Name = "Sample Product",
            Description = "This is a sample product description.",
            Price = 19.99m,
            CreatedAt = new DateTime(2023, 1, 1),
            UpdatedAt = new DateTime(2023, 1, 1),
        });
        await context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk()
    {

        // Arrange
        var entity = await InitDataAsync();
        var id = entity.Id;
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/products/{id}");

        // Act
        var response = await HttpClient!.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnOk()
    {
        // Arrange
        var createProductRequest = new CreateProductRequest()
        {
            Description = "test",
            Name = "test",
            Price = decimal.One
        };
        
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(
                JsonSerializer.Serialize(createProductRequest),
                Encoding.UTF8,
                "application/json"),
            RequestUri = new Uri($"/api/products"),
        };

        // Act
        var response = await HttpClient!.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnNotFound()
    {
        var id = 0;
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/products/{id}");

        // Act
        var response = await HttpClient!.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnBadRequest_WithZeroPrice()
    {
        // Arrange
        var createProductRequest = new CreateProductRequest()
        {
            Description = "test",
            Name = "test",
            Price = decimal.MinusOne
        };
        
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(
                JsonSerializer.Serialize(createProductRequest),
                Encoding.UTF8,
                "application/json"),
            RequestUri = new Uri($"/api/products"),
        };

        // Act
        var response = await HttpClient!.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
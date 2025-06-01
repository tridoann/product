using Microsoft.AspNetCore.Mvc.Testing;
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
    [Fact]
    public async Task GetProducts_ShouldReturnOk()
    {
        var id = 1;
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/products/{id}");

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
}
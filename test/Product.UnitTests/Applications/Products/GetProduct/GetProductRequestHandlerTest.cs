using Moq;
using Xunit;
using Product.Application.Products.GetProduct;
using Microsoft.Extensions.Caching.Distributed;
using Product.Domain.Repositories;

namespace Product.UnitTests.Applications.Products.GetProduct;

public class GetProductRequestHandlerTest
{
    private readonly GetProductRequestHandler _handler;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public GetProductRequestHandlerTest()
    {
        _cacheMock = new Mock<IDistributedCache>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductRequestHandler(
            _productRepositoryMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var request = new GetProductRequest { Id = productId };
        var product = new Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.0m
        };

        _productRepositoryMock.Setup(repo => repo.GetAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _cacheMock.Setup(cache => cache.GetAsync($"Product_{productId}", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(null as byte[])); // Simulate cache miss
        _cacheMock.Setup(cache => cache.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(product.Name, response.Name);
        Assert.Equal(product.Description, response.Description);
        Assert.Equal(product.Price, response.Price);
    }


    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExistsWithCacheHit()
    {
        // Arrange
        var productId = 1;
        var request = new GetProductRequest { Id = productId };
        var response = new GetProductResponse()
        {
            Description = "someThing",
            Name = "product",
            Price = decimal.One
        };

        _cacheMock.Setup(cache => cache.GetAsync($"Product_{productId}", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(
                System.Text.Encoding.UTF8.GetBytes(
                    System.Text.Json.JsonSerializer.Serialize(
                        response)))!); // Simulate cache hit

        // Act
        var result = await _handler.Handle(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(result.Name, response.Name);
        Assert.Equal(result.Description, response.Description);
        Assert.Equal(result.Price, response.Price);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDeserializeFailWithCacheHit()
    {
        // Arrange
        var productId = 1;
        var request = new GetProductRequest { Id = productId };

        _cacheMock.Setup(cache => cache.GetAsync($"Product_{productId}", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(
                System.Text.Encoding.UTF8.GetBytes(
                    System.Text.Json.JsonSerializer.Serialize(
                        new
                        {
                            Text = 1
                        })))!); // Simulate cache hit

        // Act & Assert
        await Assert.ThrowsAsync<System.Text.Json.JsonException>(
            () => _handler.Handle(request, TestContext.Current.CancellationToken));
    }
}
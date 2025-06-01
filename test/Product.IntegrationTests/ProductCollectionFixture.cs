using Xunit;

namespace Product.IntegrationTests;

[CollectionDefinition(nameof(ProductCollectionFixture))]
public class ProductCollectionFixture : ICollectionFixture<ProductContainerFixture> { }
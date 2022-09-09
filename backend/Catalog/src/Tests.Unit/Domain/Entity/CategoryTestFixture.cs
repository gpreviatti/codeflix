using Bogus;
using Domain.Entity;
using Unit.Common;
using Xunit;

namespace Unit.Domain.Entity;

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> { }

public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base() { }
}

using Domain.Repository;
using Entity = Domain.Entity;

namespace Tests.Integration.Data.UnitOfWork;
public abstract class UnitOfWorkTestFixture : BaseFixture
{
    protected IUnitOfWork unitOfWork;

    public UnitOfWorkTestFixture()
    {
        unitOfWork = new Infra.Data.UnitOfWork(dbContext);
    }

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];

        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
            categoryDescription = categoryDescription[..10000];

        return categoryDescription;
    }

    public Entity.Category GetCategory() =>
        new(GetValidCategoryName(), GetValidCategoryDescription());

    public List<Entity.Category> GetCategories(int length = 10) =>
        Enumerable.Range(1, length).Select(_ => GetCategory()).ToList();
}

using Application.Dtos.Category;
using Application.Interfaces.UseCases;
using Bogus;
using Domain.Entity;
using Domain.SeedWork.SearchableRepository;
using CategoryUseCase = Application.UseCases.Category;

namespace Unit.Application.UseCases.UpdateCategory;

public class ListCategoriesTestFixture : CategoryBaseFixture
{
    protected readonly IListCategories _listCategories;

    public ListCategoriesTestFixture()
    {
        _listCategories = new CategoryUseCase.ListCategories(_repositoryMock.Object);
    }

    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<Category>();
        for (int i = 0; i < length; i++)
            list.Add(GetValidCategory());
        return list;
    }

    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();
        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ?
                SearchOrder.Asc : SearchOrder.Desc
        );
    }
}

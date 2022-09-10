using Application.Dtos.Category;
using Domain.Entity;
using Domain.SeedWork.SearchableRepository;
using Tests.Common.Generators.Entities;

namespace Tests.Common.Generators.Dtos;
public class ListCategoriesInputGenerator : CommonGenerator
{
    public static List<Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<Category>();

        for (int i = 0; i < length; i++)
            list.Add(CategoryGenerator.GetCategory());

        return list;
    }

    public static ListCategoriesInput GetExampleInput()
    {
        var random = new Random();
        var productName = GetFaker().Commerce.ProductName();
        var sort = random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc;

        return new(
            random.Next(1, 10),
            random.Next(15, 100),
            productName,
            productName,
            sort
        );
    }

    public static IEnumerable<object[]> GetInputsWithoutAllParameter(int times = 14)
    {
        var inputExample = GetExampleInput();
        for (int i = 0; i < times; i++)
        {
            switch (i % 7)
            {
                case 0:
                    yield return new object[] {
                        new ListCategoriesInput()
                    };
                    break;
                case 1:
                    yield return new object[] {
                        new ListCategoriesInput(inputExample.Page)
                    };
                    break;
                case 3:
                    yield return new object[] {
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage
                        )
                    };
                    break;
                case 4:
                    yield return new object[] {
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search
                        )
                    };
                    break;
                case 5:
                    yield return new object[] {
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort
                        )
                    };
                    break;
                case 6:
                    yield return new object[] { inputExample };
                    break;
                default:
                    yield return new object[] {
                        new ListCategoriesInput()
                    };
                    break;
            }
        }
    }
}

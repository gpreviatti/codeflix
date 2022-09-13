using Application.Dtos.Category;
using Domain.SeedWork.SearchableRepository;

namespace Tests.Common.Generators.Dtos;
public class ListCategoriesInputGenerator : CommonGenerator
{
    public static ListCategoriesInput GetInput()
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
        var inputExample = GetInput();
        for (var i = 0; i < times; i++)
        {
            yield return (i % 7) switch
            {
                1 => new object[] { 
                    new ListCategoriesInput(inputExample.Page) 
                },
                3 => new object[] { 
                    new ListCategoriesInput(inputExample.Page, inputExample.PerPage) 
                },
                4 => new object[] { 
                    new ListCategoriesInput(inputExample.Page, inputExample.PerPage, inputExample.Search)
                },
                5 => new object[] { 
                    new ListCategoriesInput(
                        inputExample.Page, inputExample.PerPage, 
                        inputExample.Search, inputExample.Sort
                    )
                },
                6 => new object[] { inputExample },
                _ => new object[] { new ListCategoriesInput()}
            };
        }
    }
}

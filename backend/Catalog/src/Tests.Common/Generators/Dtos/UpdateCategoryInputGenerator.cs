using Application.Dtos.Category;
using Tests.Common.Generators.Entities;

namespace Tests.Common.Generators.Dtos;
public class UpdateCategoryInputGenerator : CommonGenerator
{
    public static UpdateCategoryInput GetCategory(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetFaker().Commerce.ProductName(),
        GetFaker().Commerce.ProductDescription(),
        true
    );

    public static UpdateCategoryInput GetInvalidInputShortName()
    {
        var inputShortName = GetCategory();
        
        inputShortName.Name = inputShortName.Name[..2];

        return inputShortName;
    }

    public static UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var inputTooLongName = GetCategory();
        inputTooLongName.Name = GetFaker().Lorem.Letter(256);
        return inputTooLongName;
    }

    public static UpdateCategoryInput GetInvalidInputNameNull()
    {
        var inputNullName = GetCategory();
        inputNullName.Name = null!;
        return inputNullName;
    }

    public static UpdateCategoryInput GetInvalidInputDescriptionNull()
    {
        var inputNullDescription = GetCategory();
        inputNullDescription.Description = null!;
        return inputNullDescription;
    }

    public static UpdateCategoryInput GetInvalidInputDescriptionTooLongDescription()
    {
        var inputTooLongDescription = GetCategory();

        inputTooLongDescription.Description = GetFaker().Lorem.Letter(10001);
        
        return inputTooLongDescription;
    }

    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        for (int indice = 0; indice < times; indice++)
        {
            var exampleCategory = CategoryGenerator.GetCategory();

            var exampleInput = GetCategory(exampleCategory.Id);

            yield return new object[] {
                exampleCategory, exampleInput
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (var index = 0; index < times; index++)
        {
            invalidInputsList.Add((index % totalInvalidCases) switch
            {
                0 => new object[] {
                        GetInvalidInputShortName(),
                        "Name should be at least 3 characters"
                },
                1 => new object[] {
                        GetInvalidInputTooLongName(),
                        "Name should be less or equal 255 characters"
                    },
                2 => new object[] {
                        GetInvalidInputDescriptionTooLongDescription(),
                        "Description should be less or equal 10000 characters"
                },
                _ => Array.Empty<object>()
            });
        }

        return invalidInputsList;
    }
}

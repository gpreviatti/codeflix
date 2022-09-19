using Application.Dtos.Category;

namespace Tests.Common.Generators.Dtos;
public class CreateCategoryInputGenerator : CommonGenerator
{
    public static CreateCategoryInput GetCategoryInput() => new(
        GetFaker().Commerce.ProductName(),
        GetFaker().Commerce.ProductDescription(),
        true
    );

    public static CreateCategoryInput GetInvalidInputShortName()
    {
        var inputShortName = GetCategoryInput();
        
        inputShortName.Name = inputShortName.Name[..2];

        return inputShortName;
    }

    public static CreateCategoryInput GetInvalidInputTooLongName()
    {
        var inputTooLongName = GetCategoryInput();
        inputTooLongName.Name = GetFaker().Lorem.Letter(256);
        return inputTooLongName;
    }

    public static CreateCategoryInput GetInvalidInputNameNull()
    {
        var inputNullName = GetCategoryInput();
        inputNullName.Name = null!;
        return inputNullName;
    }

    public static CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var inputNullDescription = GetCategoryInput();
        inputNullDescription.Description = null!;
        return inputNullDescription;
    }

    public static CreateCategoryInput GetInvalidInputDescriptionTooLongDescription()
    {
        var inputTooLongDescription = GetCategoryInput();

        inputTooLongDescription.Description = GetFaker().Lorem.Letter(10001);
        
        return inputTooLongDescription;
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var inputList = new List<object[]>();
        var totalInvalidCases = 4;
        for (var i = 0; i < times; i++)
        {
            inputList.Add((i % totalInvalidCases) switch
            {
                0 => new object[] { "Name should be at least 3 characters", GetInvalidInputShortName() },
                1 => new object[] { "Name should be less or equal 255 characters", GetInvalidInputTooLongName() },
                2 => new object[] { "Name should not be empty or null", GetInvalidInputNameNull() },
                3 => new object[] { "Description should not be null", GetInvalidInputDescriptionNull() },
                4 => new object[] { "Description should be less or equal 10000 characters", GetInvalidInputDescriptionTooLongDescription() },
                _ => Array.Empty<object>()
            });
        }

        return inputList;
    }

    public static IEnumerable<object[]> GetE2eInvalidInputs()
    {
        var inputList = new List<object[]>();
        var totalInvalidCases = 4;
        for (var i = 0; i < 3; i++)
        {
            inputList.Add((i % totalInvalidCases) switch
            {
                0 => new object[] { "Name should be at least 3 characters", GetInvalidInputShortName() },
                1 => new object[] { "Name should be less or equal 255 characters", GetInvalidInputTooLongName() },
                2 => new object[] { "Description should be less or equal 10000 characters", GetInvalidInputDescriptionTooLongDescription() },
                _ => Array.Empty<object>()
            });
        }

        return inputList;
    }
}

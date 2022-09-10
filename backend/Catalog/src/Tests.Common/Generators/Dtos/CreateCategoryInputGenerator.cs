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
        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    // Nome não pode ser menor que 3 caracteres
                    inputList.Add(new object[] {
                        "Name should be at least 3 characters",
                        GetInvalidInputShortName()
                    });
                    break;
                case 1:
                    // Nome não pode ser mais que 255 caracteres
                    inputList.Add(new object[] {
                        "Name should be less or equal 255 characters",
                        GetInvalidInputTooLongName()
                    });
                    break;
                case 2:
                    // Nome não pode ser null
                    inputList.Add(new object[] {
                        "Name should not be empty or null",
                        GetInvalidInputNameNull()
                    });
                    break;
                case 3:
                    // Descricao não pode ser nula
                    inputList.Add(new object[] {
                        "Description should not be null",
                        GetInvalidInputDescriptionNull()
                    });
                    break;
                case 4:
                    // Descricao não pode ser maior que 10000 caracters
                    inputList.Add(new object[] {
                        "Description should be less or equal 10000 characters",
                        GetInvalidInputDescriptionTooLongDescription()
                    });
                    break;
            }
        }

        return inputList;
    }
}

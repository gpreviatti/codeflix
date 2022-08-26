namespace Unit.Application.UseCases.CreateCategory;

public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
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
                        fixture.GetInvalidInputShortName()
                    });
                    break;
                case 1:
                    // Nome não pode ser mais que 255 caracteres
                    inputList.Add(new object[] {
                        "Name should be less or equal 255 characters",
                        fixture.GetInvalidInputTooLongName()
                    });
                    break;
                case 2:
                    // Nome não pode ser null
                    inputList.Add(new object[] {
                        "Name should not be empty or null",
                        fixture.GetInvalidInputNameNull()
                    });
                    break;
                case 3:
                    // Descricao não pode ser nula
                    inputList.Add(new object[] {
                        "Description should not be null",
                        fixture.GetInvalidInputDescriptionNull()
                    });
                    break;
                case 4:
                    // Descricao não pode ser maior que 10000 caracters
                    inputList.Add(new object[] {
                        "Description should be less or equal 10000 characters",
                        fixture.GetInvalidInputDescriptionTooLongDescription()
                    });
                    break;
            }
        }

        return inputList;
    }
}

using Bogus;

namespace Tests.Common.Generators;
public class CommonGenerator
{
    public static Faker GetFaker() => new("pt_BR");
    public static Faker<T> GetFaker<T>() where T : class => new("pt_BR");
}

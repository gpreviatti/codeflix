using Bogus;
using Tests.Common.Generators;

namespace Tests.Unit.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() => Faker = CommonGenerator.GetFaker();

    public static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
}

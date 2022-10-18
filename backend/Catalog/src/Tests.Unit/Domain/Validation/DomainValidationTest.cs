﻿using Bogus;
using Domain.Excpetions;
using Domain.Validation;
using Tests.Common.Generators;

namespace Tests.Unit.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = CommonGenerator.GetFaker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = Faker.Commerce.ProductName();
        var action = () => DomainValidation.NotNull(value, fieldName);
        _=action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNull(null, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }


    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        _ = action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        _=action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.MinLength(target, minLength, fieldName);

        _=action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 10 };
        var faker = CommonGenerator.GetFaker();
        for (var i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + new Random().Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.MinLength(target, minLength, fieldName);

        _=action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = CommonGenerator.GetFaker();

        for (var i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

        _=action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 5 };
        var faker = CommonGenerator.GetFaker();
        for (var i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

        _=action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = CommonGenerator.GetFaker();
        for (var i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + new Random().Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }
}

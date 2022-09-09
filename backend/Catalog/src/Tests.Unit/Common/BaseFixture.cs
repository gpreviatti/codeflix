﻿using Bogus;

namespace Unit.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture() => Faker = new Faker("pt_BR");

    public static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
}
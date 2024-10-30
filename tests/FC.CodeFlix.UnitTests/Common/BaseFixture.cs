﻿using Bogus;

namespace FC.CodeFlix.Catalog.UnitTests.Common;

public abstract class BaseFixture
{
    protected BaseFixture() => Faker = new Faker("en");

    public Faker Faker { get; set; }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}
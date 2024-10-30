using Bogus;

namespace FC.CodeFlix.Catalog.Tests.Shared;
public abstract class DataGeneratorBase
{
    protected DataGeneratorBase() => Faker = new Faker("en");

    public Faker Faker { get; set; }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}
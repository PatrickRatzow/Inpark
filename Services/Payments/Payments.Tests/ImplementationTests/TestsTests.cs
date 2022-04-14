using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zoo.Payments.Tests.ImplementationTests;

[TestFixture]
[Category(Categories.Implementation)]
public class TestsTests
{
    private static readonly Assembly Assembly = typeof(TestsTests).Assembly;
    
    public static IEnumerable<Type> GetTestFixturesTests()
    {
        return Assembly
            .GetExportedTypes()
            .Where(t => t.GetCustomAttribute<TestFixtureAttribute>() is not null 
                        || (bool)t.FullName?.EndsWith("Tests"));
    }

    [TestCaseSource(nameof(GetTestFixturesTests))]
    public void Test_ShouldHaveA_Category(Type @class)
    {
        // Arrange
        var hasCategoryAttribute = @class.GetCustomAttributes<CategoryAttribute>().Any();

        // Assert
        hasCategoryAttribute.Should().BeTrue();
    }
    
    public static IEnumerable<Type> GetTests()
    {
        return Assembly
            .GetExportedTypes()
            .Where(t => (bool)t.FullName?.EndsWith("Tests"));
    }
    
    [TestCaseSource(nameof(GetTests))]
    public void Test_ShouldHaveA_TestFixture(Type @class)
    {
        // Arrange
        var hasTestFixture = @class.GetCustomAttributes<TestFixtureAttribute>().Any();

        // Assert
        hasTestFixture.Should().BeTrue();
    }
}
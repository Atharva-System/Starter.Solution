using System.Reflection;
using MediatR;
using NetArchTest.Rules;

namespace Starter.ArhitectureTests;
public class ApplicationTests : BaseTest
{
    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        // Arrange
        var assembly = Assembly.Load(ApplicationNamespace);

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }

    [Fact]
    public void Mediatr_RequestHandlers_Should_BeSealed()
    {
        // Arrange
        var assembly = Assembly.Load(ApplicationNamespace);

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IBaseRequest))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }
}

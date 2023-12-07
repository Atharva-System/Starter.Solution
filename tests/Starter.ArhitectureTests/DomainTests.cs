using System.Reflection;
using NetArchTest.Rules;
using Starter.Domain.Common;

namespace Starter.ArhitectureTests;
public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEntities_Should_BeSealed()
    {
        // Arrange
        var assembly = Assembly.Load(DomainNamespace);

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(BaseAuditableEntity))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        Assert.True(testResult.IsSuccessful);
    }
}

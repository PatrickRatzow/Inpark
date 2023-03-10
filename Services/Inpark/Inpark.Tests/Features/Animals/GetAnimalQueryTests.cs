using System.Threading.Tasks;
using DomainFixture;
using Zeta.Inpark.Entities;
using Zeta.Inpark.Errors;
using Zeta.Inpark.Features.Animals;

namespace Zeta.Inpark.Tests.Features.Animals;

[TestFixture]
[Category(Categories.Integration)]
public class GetAnimalQueryTests : TestBase
{
    [Test]
    public async Task Handle_ShouldReturnAnimal_WhenAnAnimalExists()
    {
        // Arrange
        var animal = await Add(Fixture.Valid<Animal>().Create());
        var query = new GetAnimalQuery(animal.Name.LatinName);
        
        // Act
        var response = await Send(query);

        // Assert
        response.Value.Should().BeOfType<Animal>();
        var result = response.Value.As<Animal>();
        result.Id.Should().Be(animal.Id);
    }
    
    [Test]
    public async Task Handle_ShouldReturnAnimalNotFound_WhenAnAnimalDoesNotExist()
    {
        // Arrange
        var query = new GetAnimalQuery("floppa");
        
        // Act
        var response = await Send(query);

        // Assert
        response.Value.Should().BeOfType<AnimalNotFound>();
    }
}
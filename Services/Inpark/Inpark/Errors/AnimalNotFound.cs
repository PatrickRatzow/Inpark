using Zeta.Common;

namespace Zeta.Inpark.Errors;

public record AnimalNotFound(string Name) : INotFoundError
{
    public string ErrorMessage => $"There is no animal with the latin name {Name}";
}
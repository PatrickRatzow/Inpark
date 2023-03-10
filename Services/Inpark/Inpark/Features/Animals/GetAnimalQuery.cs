using Microsoft.EntityFrameworkCore;
using Zeta.Common.Api;
using Zeta.Inpark.Entities;
using Zeta.Inpark.Errors;

namespace Zeta.Inpark.Features.Animals;

public record GetAnimalQuery(string LatinName) : IRequest<OneOf<Animal, AnimalNotFound>>;

public class GetAnimalQueryHandler : IRequestHandler<GetAnimalQuery, OneOf<Animal, AnimalNotFound>>
{
    private readonly InparkDbContext _context;

    public GetAnimalQueryHandler(InparkDbContext context)
    {
        _context = context;
    }

    //TODO change this to return AnimalDto
    public async Task<OneOf<Animal, AnimalNotFound>> Handle(GetAnimalQuery request, CancellationToken cancellationToken)
    {
        var animal = await _context.Animals
            .FirstOrDefaultAsync(x => x.Name.LatinName == request.LatinName, cancellationToken);
        if (animal is null) return new AnimalNotFound(request.LatinName);

        return animal;
    }
}

public class GetAnimalQueryValidator : AbstractValidator<GetAnimalQuery>
{
    public GetAnimalQueryValidator()
    {
        RuleFor(x => x.LatinName).NotEmpty().MaximumLength(512);
    }
}

[ApiController]
[MethodGroup(Groups.Animals)]
[ResponseCache(Duration = 43200)]
public partial class GetAnimalController : ZooController
{
    private readonly IMediator _mediator;

    public GetAnimalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets an animal in the park
    /// </summary>
    [HttpGet("animals/{latinName}")]
    public async partial Task<ActionResult> GetAnimal([FromRoute] string latinName, CancellationToken cancellationToken)
    {
        var query = new GetAnimalQuery(latinName);
        var result = await _mediator.Send(query, cancellationToken);

        return Map(result);
    }
}
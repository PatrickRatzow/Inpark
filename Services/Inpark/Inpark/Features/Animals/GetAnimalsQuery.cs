using Microsoft.EntityFrameworkCore;
using Zeta.Common.Api;
using Zeta.Inpark.Contracts;
using Zeta.Inpark.Features.Animals.Interfaces;
using Zeta.Inpark.Models;

namespace Zeta.Inpark.Features.Animals;

public record GetAnimalsQuery : IRequest<OneOf<List<AnimalDto>>>;

public class GetAnimalsHandler : IRequestHandler<GetAnimalsQuery, OneOf<List<AnimalDto>>>
{
    private readonly InparkDbContext _context;
    private readonly IAnimalMapper _mapper;

    public GetAnimalsHandler(InparkDbContext context, IAnimalMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OneOf<List<AnimalDto>>> Handle(GetAnimalsQuery request, CancellationToken cancellationToken)
    {
        var animals = await _context.Animals
            .Include(x => x.Areas)
            .OrderBy(x => x.Name.DisplayName)
            .ToListAsync(cancellationToken);
        var animalDtos = animals.Select(x => {
            var name = new AnimalNameDto(x.Name.DisplayName, x.Name.LatinName);
            var image = new ImagePairDto(x.Image.PreviewUrl, x.Image.FullscreenUrl);
            var status = (IUCNStatusDto) x.Status;
            if (!_mapper.ParseContent(x.Content).IsSuccess(out var content))
                throw new InvalidDataException("Unable to parse content");

            return new AnimalDto(
                name,
                x.Category,
                image,
                status,
                x.Id.ToString(),
                content!.Select(MapToContentElementDto).ToList(),
                x.Areas.Count != 0
            );
        });
    
        return animalDtos.ToList();
    }
    
    private static ContentDto MapToContentElementDto(IContent content)
    {
        return new(
            content.Value,
            content.Type,
            content.Children.Select(MapToContentElementDto).ToList()
        );
    }
}

[ApiController]
[MethodGroup(Groups.Animals)]
public partial class GetAnimalsController : ZooController
{
    private readonly IMediator _mediator;

    public GetAnimalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all animals in the park.
    /// </summary>
    [HttpGet("animals")]
    public async partial Task<ActionResult> GetAnimals(CancellationToken cancellationToken)
    {
        var command = new GetAnimalsQuery();
        var result = await _mediator.Send(command, cancellationToken);

        return Map(result);
    }
}
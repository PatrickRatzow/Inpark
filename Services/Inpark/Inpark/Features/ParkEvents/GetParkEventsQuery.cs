﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Inpark.Contracts;
using Zoo.Inpark.Features.Events.Interfaces;
using Zoo.Inpark.Models;
using Zoo.Inpark.ValueObjects;

namespace Zoo.Inpark.Features.Events;

public record GetParkEventsQuery : IRequest<OneOf<List<ParkEventDto>>>;
public class GetParkEventsQueryQueryHandler :
    IRequestHandler<GetParkEventsQuery, OneOf<List<ParkEventDto>>>
{
    private readonly InparkDbContext _context;
    private readonly IParkEventMapper _mapper;

    public GetParkEventsQueryQueryHandler(InparkDbContext context, IParkEventMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OneOf<List<ParkEventDto>>> Handle(GetParkEventsQuery request, CancellationToken cancellationToken)
    {
        var parkEvents = await _context.ParkEvents
            .OrderBy(x => x.Range.Start)
            .ToListAsync(cancellationToken);

        var parkEventDtos = parkEvents.Select(x =>
        {
            var image = new ImagePairDto(x.Image.PreviewUrl, x.Image.FullscreenUrl);

            if (!_mapper.ParseContent(x.Content).IsSuccess(out var content))
                throw new InvalidDataException("Unable to parse content");

            var parkEventDescription = new List<IContent>();
            var parkEventProgram = new List<IContent>();

            //Seperation of the description of and event and the program
            for (int i = 0; i < content.Count(); i++)
            {
                if (content[i].Value.ToString() != "Program")
                    parkEventDescription.Add(content[i]);
                else
                {
                    parkEventProgram.Add(content[i]);
                    parkEventProgram.Add(content[i + 1]);
                    i++;
                }
            }

            var asd = parkEventDescription!.Select(MapToContentDto).ToList();
            var sdf = parkEventProgram!.Select(MapToContentDto).ToList();
            return new ParkEventDto(
                Guid.NewGuid(),
                image,
                x.Title,
                x.Range.Start,
                x.Range.End,
                parkEventDescription!.Select(MapToContentDto).ToList(),
                parkEventProgram!.Select(MapToContentDto).ToList()
            );
        }).Where(x => x.Start >= DateTime.Today);

        return parkEventDtos.ToList();
    }

    private static ContentDto MapToContentDto(IContent content)
    {
        return new(
            content.Value,
            content.Type,
            content.Children.Select(MapToContentDto).ToList()
        );
    }
}

[ApiController]
[MethodGroup(Groups.ParkEvents)]
public partial class ParkEventsController : ZooController
{
    private readonly IMediator _mediator;

    public ParkEventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all events in the park.
    /// </summary>
    [HttpGet("park-events")]
    public async partial Task<ActionResult> GetParkEvents(CancellationToken cancellationToken)
    {
        var command = new GetParkEventsQuery();
        var result = await _mediator.Send(command, cancellationToken);

        return Map(result);
    }
}



using System.Text.Json;
using Microsoft.Extensions.Logging;
using Zoo.Inpark.Entities;
using Zoo.Inpark.Enums;
using Zoo.Inpark.Extensions;
using Zoo.Inpark.Features.OpeningHours.Interfaces;
using Zoo.Inpark.ValueObjects;

namespace Zoo.Inpark.Features.OpeningHours.AalborgZoo;

public class AalborgZooOpeningHoursMapper : IOpeningHoursMapper
{
    private readonly ILogger<AalborgZooOpeningHoursMapper> _logger;
    
    public AalborgZooOpeningHoursMapper(ILogger<AalborgZooOpeningHoursMapper> logger)
    {
        _logger = logger;
    }

    public Result<List<OpeningHour>, string> Parse(string input)
    {
        try
        {
            var openingHours = new List<OpeningHour>();
            using var json = JsonDocument.Parse(input);
            var items = json.RootElement.GetProperty("items");
            foreach (var itemObj in items.EnumerateArray())
            {
                var item = itemObj.GetProperty("item");
                var properties = item.GetProperty("properties");
                var times = properties.GetProperty("times").Deserialize<List<OpeningTime>>(new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });;

                foreach (var time in times!)
                {
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
                    var start = time.Start.ToTimeZoneOffset(timeZone);
                    var end = time.End.ToTimeZoneOffset(timeZone);
                    var timeRange = TimeRange.From(start, end);
                    var open = time.OpenClosed is "open";
                    var header = time.Header.Replace("Aalborg Zoo -", "").Trim();
                    var days = time.WeekDays.ToHashSet();
                    var day = days.ToWeekDay();

                    var openingHour = OpeningHour.Create(Guid.NewGuid(), header, timeRange, day, open);
                    
                    openingHours.Add(openingHour);
                }
            }
            return openingHours;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occured while mapping. Exception: {Exception}", ex);

            return "Failed to map";
        }
    }

    private class OpeningTime
    {
        public bool Special { get; set; }
        public string OpenClosed { get; set; } = null!;
        public List<string> WeekDays { get; set; } = new();
        public DateTimeOffset Start { get; set; }
        public string Header { get; set; } = null!;
        public DateTimeOffset End { get; set; }
    }
}
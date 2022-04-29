namespace Zoo.Inpark.Contracts;

public record OpeningHourDto(
    string Name,
    DateTime Start,
    DateTime End,
    bool Open,
    List<string> Days
);
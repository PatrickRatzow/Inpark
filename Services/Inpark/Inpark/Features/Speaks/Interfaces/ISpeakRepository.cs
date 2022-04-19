using Zoo.Inpark.ValueObjects;

namespace Zoo.Inpark.Features.Speaks.Interfaces;

public interface ISpeakRepository
{
    public interface ISpeaksRepository
    {
        ValueTask<Result<string, string>> GetRange(TimeRange range);
    }
}
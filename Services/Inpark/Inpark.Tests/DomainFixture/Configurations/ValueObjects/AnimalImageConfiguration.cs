using DomainFixture;
using Zeta.Inpark.ValueObjects;

namespace Zeta.Inpark.Tests.DomainFixture.Configurations.ValueObjects;

public class AnimalImageConfiguration : FixtureConfiguration<ImagePair>
{
    public override void Configure()
    {
        Property(x => x.PreviewUrl)
            .Length(1).IsValid()
            .Empty().IsInvalid();
        
        Property(x => x.FullscreenUrl)
            .Length(1).IsValid()
            .Empty().IsInvalid();
    }
}
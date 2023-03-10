namespace Zeta.Inpark.Contracts;

public record AnimalDto(
    AnimalNameDto Name,
    string Category,
    ImagePairDto Image,
    IUCNStatusDto Status,
    string Id,
    List<ContentDto> Contents,
    bool HasMap
);
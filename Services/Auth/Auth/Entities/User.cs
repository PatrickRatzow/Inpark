using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.Inpark.Auth.ValueObjects;

namespace Zeta.Inpark.Auth.Entities;

public class User : Entity
{
    public Guid Id { get; private set; }
    public EmailAddress EmailAddress { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public DateTime LastSeen { get; private set; }
    
    private List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens
    {
        get => _refreshTokens;
        set => _refreshTokens = (List<RefreshToken>)value;
    }

    private User() { }

    public static User Create(Guid id, EmailAddress emailAddress, Password password)
    {
        var instance = new User
        {
            Id = id,
            EmailAddress = emailAddress,
            Password = password,
            LastSeen = DateTime.Now,
        };
        instance.Validate();

        return instance;
    }

    public RefreshToken CreateRefreshToken()
    {
        var expiresAt = DateTime.Now.AddDays(30);
        var refreshToken = RefreshToken.From(expiresAt);
        
        _refreshTokens.Add(refreshToken);
        
        return refreshToken;
    }
    
    public void RemoveRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Remove(refreshToken);
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EmailAddress).NotNull();
        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.LastSeen).NotEmpty();
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.RefreshTokens)
            .WithOne();
        
        builder.OwnsOne(x => x.Password, x =>
        {
            x.Property(y => y.Hash).HasColumnName("Password_Hash");
            x.Property(y => y.Salt).HasColumnName("Password_Salt");
        });
        builder.OwnsOne(x => x.EmailAddress, x =>
        {
            x.Property(y => y.Value).HasColumnName("Email");
        });
    }
}
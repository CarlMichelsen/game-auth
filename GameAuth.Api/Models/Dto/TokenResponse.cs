namespace GameAuth.Api.Models.Dto;

public record TokenResponse
{
    public string IdentityToken { get; set; } = string.Empty;
}
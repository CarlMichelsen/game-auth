namespace GameAuth.Api.Models.Dto;

public record TokenResponse
{
    public string AuthToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
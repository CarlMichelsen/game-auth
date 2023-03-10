namespace GameAuth.Api.Models.Dto.Login;

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
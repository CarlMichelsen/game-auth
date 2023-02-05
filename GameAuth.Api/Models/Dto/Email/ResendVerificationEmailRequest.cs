namespace GameAuth.Api.Models.Dto.Email;

public record ResendVerificationEmailRequest
{
    public required string Email { get; set; }
}
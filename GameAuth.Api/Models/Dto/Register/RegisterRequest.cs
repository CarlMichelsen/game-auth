namespace GameAuth.Api.Models.Dto.Register;

public record RegisterRequest
{
    public required string Email { get; set; }
    public required string MatchingEmail { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string MatchingPassword { get; set; }
    public required Address Address { get; set; }
}
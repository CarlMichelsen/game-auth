using System.ComponentModel.DataAnnotations.Schema;

namespace GameAuth.Database.Models.Entities;

public class VerificationEmail
{
    public long Id { get; set; }
    [ForeignKey("Account")]
    public required long AccountId { get; set; }
    [ForeignKey("Email")]
    public required long EmailId { get; set; }
    public required string Code { get; set; }
    public required DateTime Sent { get; set; }
}
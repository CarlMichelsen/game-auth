using System.ComponentModel.DataAnnotations.Schema;

namespace GameAuth.Database.Models.Entities;

public class Email
{
    public long Id { get; set; }
    [ForeignKey("Account")]
    public long AccountId { get; set; }
    public required string Value { get; set; }
    public required bool IsPrimary { get; set; }
    public required DateTime Added { get; set; }
    public required DateTime? LastSetToPrimary { get; set; }
}
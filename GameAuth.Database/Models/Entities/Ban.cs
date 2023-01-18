using System.ComponentModel.DataAnnotations.Schema;

namespace GameAuth.Database.Models.Entities;

public class Ban
{
    public long Id { get; set; }

    [ForeignKey("Account")]
    public required long AccountId { get; set; }
    public required string Reason { get; set; }
    public required DateTime BanTime { get; set; }
}
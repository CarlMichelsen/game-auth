using System.ComponentModel.DataAnnotations.Schema;

namespace GameAuth.Database.Models.Entities;

public class Account
{
    public long Id { get; set; }
    public required ICollection<Email> Emails { get; set; }
    public required bool EmailVerified { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required Address Address { get; set; }
    public required DateTime LastModified { get; set; }
}
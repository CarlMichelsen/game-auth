namespace GameAuth.Database.Models.Entities;

public class Email
{
    public long Id { get; set; }
    public required string Value { get; set; }
    public bool Verified { get; set; }
    public required DateTime Added { get; set; }
    public required DateTime? LastSetToPrimary { get; set; }
}
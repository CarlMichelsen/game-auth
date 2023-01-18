namespace GameAuth.Database.Models.Entities;

public class Address
{
    public long Id { get; set; }
    public required string CountryOrRegion { get; set; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required string City { get; set; }
    public required string StateProvinceOrRegion { get; set; }
    public required string ZipOrPostalCode { get; set; }
    public required DateTime LastModified { get; set; }
}
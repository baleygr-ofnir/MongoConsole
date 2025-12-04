namespace MongoConsole.Models;

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public override string ToString()
    {
        return $"Street: {Street}, City: {City}, State: {State}, Zip Code: {ZipCode}";
    }
}
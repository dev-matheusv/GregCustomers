namespace GregCustomers.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public string Street { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Address() { }

    public Address(Guid clientId, string street)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
        SetStreet(street);

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateStreet(string street)
    {
        SetStreet(street);
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required.", nameof(street));

        Street = street.Trim();
    }
}
using System.Text.RegularExpressions;

namespace GregCustomers.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public byte[]? Logo { get; private set; }
    public string? LogoContentType { get; private set; }
    public string? LogoFileName { get; private set; }

    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Client() { } // para EF se você decidir mapear leitura por EF

    public Client(string name, string email)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetEmail(email);

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void Update(string name, string email)
    {
        SetName(name);
        SetEmail(email);
        Touch();
    }

    public void SetLogo(byte[] logo, string contentType, string fileName)
    {
        Logo = logo ?? throw new ArgumentNullException(nameof(logo));
        LogoContentType = contentType;
        LogoFileName = fileName;
        Touch();
    }

    public void ClearLogo()
    {
        Logo = null;
        LogoContentType = null;
        LogoFileName = null;
        Touch();
    }

    public void AddAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        _addresses.Add(address);
        Touch();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        email = email.Trim();

        // validação leve (o grosso fica no FluentValidation)
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Email is invalid.", nameof(email));

        Email = email;
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
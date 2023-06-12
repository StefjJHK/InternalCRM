namespace BIP.InternalCRM.Domain.ValueObjects;

public record ContactInfo
{
    private ContactInfo(
        string fullname,
        string phoneNumber,
        string email)
    {
        Fullname = fullname;
        PhoneNumber = phoneNumber;
        Email = email;
    }
    
    public string Fullname { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public string Email { get; private set; }
    
    public static ContactInfo Create(
        string fullname,
        string phoneNumber,
        string email)
    {
        var @new = new ContactInfo(fullname, phoneNumber, email);

        return @new;
    }
}

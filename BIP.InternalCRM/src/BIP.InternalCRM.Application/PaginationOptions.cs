namespace BIP.InternalCRM.Application;

public record PaginationOptions(
    int PageNumber = 0,
    int Count = 0
)
{
    public static readonly PaginationOptions Empty = new(0, 0);
}
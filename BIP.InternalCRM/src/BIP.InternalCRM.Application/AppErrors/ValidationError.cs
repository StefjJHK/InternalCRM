using BIP.InternalCRM.Primitives.ErrorHandling;

namespace BIP.InternalCRM.Application.AppErrors;

public record ValidationError(
    string Code,
    string Message
) : Error(Code, Message);

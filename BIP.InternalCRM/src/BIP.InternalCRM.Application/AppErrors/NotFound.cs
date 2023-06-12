using BIP.InternalCRM.Primitives.ErrorHandling;

namespace BIP.InternalCRM.Application.AppErrors;

public record NotFound(
    string Code,
    string Message
) : Error(Code, Message);

public record NotFound<TEntity>() : NotFound(
    $"{typeof(TEntity).Name}.NotFound",
    $"The {typeof(TEntity).Name} was not found");

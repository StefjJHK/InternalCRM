namespace BIP.InternalCRM.Application.AppErrors;

public static class AppValidationErrors
{
    public static readonly ValidationError IsRequired = new(
        "{PropertyName}.IsRequired",
        "{PropertyName} is mandatory");

    public static readonly ValidationError AtLeastOneIsRequired = new(
        "{PropertyName}.AtLeastOneIsRequired",
        "{PropertyName} at least one is required");
    
    public static readonly ValidationError MustBeGreaterThanZero = new(
        "{PropertyName}.MustBeGreaterThanZero",
        "{PropertyName} must be greater than zero");
    
    public static readonly ValidationError DateTimeMustBeConsistent = new(
        "{PropertyName}.DateTimeMustBeConsistent",
        "{PropertyName} must be consistent");
}
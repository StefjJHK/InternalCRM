using System.Reflection;

namespace BIP.InternalCRM.Domain;

public static class DomainProject
{
    public static Assembly AssemblyRef =>
        typeof(DomainProject).Assembly;
}
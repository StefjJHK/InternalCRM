using System.Reflection;

namespace BIP.InternalCRM.Application;

public static class ApplicationProject
{
    public static Assembly AssemblyRef
        => typeof(ApplicationProject).Assembly;
}

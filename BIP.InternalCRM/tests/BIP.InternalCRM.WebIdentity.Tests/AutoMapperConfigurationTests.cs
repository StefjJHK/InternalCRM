using AutoMapper;
using Xunit;

namespace BIP.InternalCRM.WebIdentity.Tests;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(WebIdentityProject.AssemblyRef);
        });

        config.AssertConfigurationIsValid();
    }
}

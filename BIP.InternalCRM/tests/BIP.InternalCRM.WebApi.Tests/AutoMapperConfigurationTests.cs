using AutoMapper;
using Xunit;

namespace BIP.InternalCRM.WebApi.Tests;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(WebApiProject.AssemblyRef);
        });

        config.AssertConfigurationIsValid();
    }
}

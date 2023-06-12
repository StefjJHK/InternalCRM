using AutoFixture;
using AutoMapper;

namespace BIP.UnitTesting.Core;

public abstract class ProfileTestsBase
{
    protected IMapper Mapper { get; private set; }

    protected MapperConfiguration Configuration { get; private set; }

    protected IFixture Fixture { get; private set; }

    protected ProfileTestsBase(bool manualMapperInit = false)
    {
        Fixture = new Fixture();

        if (!manualMapperInit)
        {
            InitializeMapper();
        }
    }

    protected void InitializeMapper(Action<IMapperConfigurationExpression> configure = null)
    {
        Configuration = new MapperConfiguration(cfg =>
        {
            configure?.Invoke(cfg);
        });
        Mapper = new Mapper(Configuration);
    }
}

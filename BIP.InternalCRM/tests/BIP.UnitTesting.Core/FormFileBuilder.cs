using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace BIP.UnitTesting.Core;

public class FormFileBuilder : ISpecimenBuilder
{
    private readonly byte[] _data;
    private readonly string _filename;

    public FormFileBuilder(string filename, byte[] data)
    {
        _data = data;
        _filename = filename;
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(IFormFile))
        {
            var iFormFile = Substitute.For<IFormFile>();
            iFormFile.OpenReadStream().Returns(new MemoryStream(_data));
            iFormFile.FileName.Returns(_filename);
            return iFormFile;
        }

        return new NoSpecimen();
    }
}

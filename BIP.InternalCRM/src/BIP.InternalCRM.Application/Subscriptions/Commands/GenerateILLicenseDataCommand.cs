using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;
using MediatR;

namespace BIP.InternalCRM.Application.Subscriptions.Commands;

public record GenerateIlLicenseDataCommand(
    ProductId ProductId,
    CustomerId CustomerId,
    DateTime CurrentTime,
    IntelliLockProject IlProject
) : IRequest<GenerateIlLicenseDataCommand.CommandResult>
{
    public record CommandResult(
        Guid Key,
        byte[] IlLicenseData
    );

    public class Handler :
        IRequestHandler<GenerateIlLicenseDataCommand, CommandResult>
    {
        private readonly int[] _randomPrimeNums = { 139, 109, 163, 181, 151 };

        public Task<CommandResult> Handle(
            GenerateIlLicenseDataCommand request,
            CancellationToken cancellationToken)
        {
            var guidBytes = new[]
            {
                Convert.ToByte(Math.Abs(request.ProductId.GetHashCode() % _randomPrimeNums[0])),
                Convert.ToByte(Math.Abs(request.CustomerId.GetHashCode() % _randomPrimeNums[2])),
                Convert.ToByte(request.CurrentTime.Ticks % _randomPrimeNums[4])
            };

            var key = new Guid(guidBytes
                .Concat(Guid.NewGuid()
                    .ToByteArray()
                    .Take(16 - guidBytes.Length))
                .ToArray());

            var ilProject = new IntelliLock.LicenseManager.ProjectFile(request.IlProject.Data);
            //ilProject.LicenseInformation.Add("product_key", key);
            var ilLicData = IntelliLock.LicenseManager.LicenseGenerator.CreateLicenseFile(ilProject);

            return Task.FromResult(new CommandResult(key, ilLicData));
        }
    }
}
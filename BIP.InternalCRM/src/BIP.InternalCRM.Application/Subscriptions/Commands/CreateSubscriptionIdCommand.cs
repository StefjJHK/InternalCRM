using Fare;
using MediatR;
using Microsoft.Extensions.Options;

namespace BIP.InternalCRM.Application.Subscriptions.Commands;

public record CreateSubscriptionIdCommand(
    IEnumerable<string> OtherSubsNumbers
) : IRequest<string>
{
    public class Handler :
        IRequestHandler<CreateSubscriptionIdCommand, string>
    {
        private readonly SubscriptionNumberOptions _options;

        public Handler(IOptions<SubscriptionNumberOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }


        public Task<string> Handle(CreateSubscriptionIdCommand request, CancellationToken cancellationToken)
        {
            var xeger = new Xeger(
                _options.Pattern,
                new Random((int)DateTime.UtcNow.Ticks % int.MaxValue));


            var subNumber = xeger.Generate();
            while (request.OtherSubsNumbers.Contains(subNumber))
            {
                subNumber = xeger.Generate();
            }
            
            return Task.FromResult(subNumber);
        }
    }
}
using ExpensesManager.Application.Commands;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Interfaces;
using ExpensesManager.Infrastructure.Interfaces;
using MediatR;


namespace ExpensesManager.Application.Handlers;

public class LogoutCommandHandler(
    IBlackListRepository blackListRepository,
    IJwtService jwtService): IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var decodedToken = jwtService.DecodeAccessToken(request.AccessToken);
        var isAlreadyBlackListed = await blackListRepository.IsBlacklistedAsync(decodedToken.jti);

        if (isAlreadyBlackListed)
        {
            throw new Exception("Access Token is already on the black list");
        }

        var token = new BlackListedToken
        {
            Id = Guid.NewGuid(),
            Jti = decodedToken.jti,
            Expiry = decodedToken.expiry,
        };
        await blackListRepository.AddAsync(token);

        return Unit.Value;
    }
}
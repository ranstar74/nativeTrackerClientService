using System.Text.RegularExpressions;
using Grpc.Core;
using nativeTrackerClientService.Credentials;
using nativeTrackerClientService.Entities;

namespace nativeTrackerClientService.Services;

public class ClientService : nativeTrackerClientService.ClientService.ClientServiceBase
{
    private readonly Regex _loginRegex = new Regex(
        @"^[\dA-Za-z_@!]+$");

    private readonly Regex _passwordRegex = new Regex(
        @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=])(?=\S+$).{8,}$");

    public override async Task<CreateAccountResponse> CreateAccount(
        CreateAccountRequest request,
        ServerCallContext context)
    {
        return await Task.Run(async () =>
        {
            await using nativeContext db = new();

            var status = CreateStatus.Created;
            if (!_loginRegex.IsMatch(request.Login))
            {
                status = CreateStatus.LoginFormatIsNotValid;
            }
            else if (!_passwordRegex.IsMatch(request.Password))
            {
                status = CreateStatus.PasswordFormatIsNotValid;
            }
            else if (request.Login == request.Password)
            {
                status = CreateStatus.LoginMustBeDifferentFromPassword;
            }
            else if (await db.ClientUsers.FindAsync(request.Login) != null)
            {
                status = CreateStatus.LoginTaken;
            }

            if (status == CreateStatus.Created)
            {
                var clientUser = new ClientUser()
                {
                    Login = request.Login,
                    CreateDate = DateTime.UtcNow
                };
                clientUser.Password = AuthorizationManager.EncryptPasswordWithClient(
                    clientUser, request.Password);
                
                await db.ClientUsers.AddAsync(clientUser);
                await db.SaveChangesAsync();
            }
            
            return new CreateAccountResponse()
            {
                Status = status
            };
        });
    }

    public override async Task<LoginAccountResponse> LoginAccount(
        LoginAccountRequest request,
        ServerCallContext context)
    {
        return await Task.Run(async () =>
        {
            var (authorized, token) = await AuthorizationManager.GenerateJwtToken(
                request.Login, request.Password);

            return new LoginAccountResponse()
            {
                Authorized = authorized,
                Token = token
            };
        });
    }
}
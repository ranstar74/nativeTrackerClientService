using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using nativeTrackerClientService.Entities;

namespace nativeTrackerClientService.Credentials
{
    public static class AuthorizationManager
    {
        public static readonly JwtSecurityTokenHandler JwtTokenHandler = new();
        public static readonly SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes("_Native_Track_SC_AM_"));

        private static readonly string Issuer = "_Native_Track_Issuer_";
        private static readonly string Audience = "_Native_Track_Audience_Clients_";
        private static readonly string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
        private static readonly TokenValidationParameters ValidationParameters;

        static AuthorizationManager()
        {
            ValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = SecurityKey,
                ValidAlgorithms = new List<string>()
                {
                    SecurityAlgorithm
                }
            };
        }

        public static async Task<(bool authorized, string token)> GenerateJwtToken(string login, string password)
        {
            await using nativeContext context = new();

            var user = await context.ClientUsers.FindAsync(login);
            // User with given login doesn't exists
            if (user == null)
            {
                return (false, string.Empty);
            }

            string passwordHash = EncryptPasswordWithClient(user, password);

            // User password doesn't match
            if (user.Password != passwordHash)
            {
                return (false, string.Empty);
            }

            var claims = new[] { new Claim(ClaimTypes.Name, login) };
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithm);
            var token = new JwtSecurityToken(Issuer, Audience, claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);
            return (true, JwtTokenHandler.WriteToken(token));
        }

        public static bool IsTokenValid(string token)
        {
            var result = JwtTokenHandler.ValidateToken(token, ValidationParameters, out _);
            return result.Identity != null;
        }

        public static string EncryptPasswordWithClient(ClientUser user, string password)
        {
            string passwordWithSalt = password + user.CreateDate.ToString("yy-MM-dd-hh-MM-ss");

            var crypt = SHA256.Create();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
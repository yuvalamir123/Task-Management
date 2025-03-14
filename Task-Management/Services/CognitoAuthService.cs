using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Security.Cryptography;
using System.Text;

namespace Task_Management.Services
{
    public class CognitoAuthService
    {
        private readonly string? _appClientId;
        private readonly string? _clientSecret;
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;

        public CognitoAuthService(IConfiguration configuration)
        {
            var awsOptions = configuration.GetSection("AWS");
            _appClientId = awsOptions["AppClientId"];
            _clientSecret = awsOptions["ClientSecret"];

            _cognitoClient = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(awsOptions["Region"]));
        }

        public async Task<string?> AuthenticateUser(string username, string password)
        {
            try
            {
                var request = new InitiateAuthRequest
                {
                    ClientId = _appClientId,
                    AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                    AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "PASSWORD", password },
                    { "SECRET_HASH", CalculateSecretHash(username) }
                }
                };

                var response = await _cognitoClient.InitiateAuthAsync(request);

                return response.AuthenticationResult?.AccessToken; // Return the JWT token
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                return null;
            }
        }

        private string CalculateSecretHash(string username)
        {
            var message = Encoding.UTF8.GetBytes(username + _appClientId);
            var key = Encoding.UTF8.GetBytes(_clientSecret);

            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(message);
            return Convert.ToBase64String(hash);
        }
    }

}

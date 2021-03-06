using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ScaleUnitManagement.WorkloadSetupOrchestrator.Utilities
{
    public class OAuthHelper
    {
        /// <summary>
        /// The header to use for OAuth authentication.
        /// </summary>
        public const string OAuthHeader = "Authorization";

        /// <summary>
        /// Retrieves an authentication header from the service.
        /// </summary>
        /// <returns>The authentication header for the Web API call.</returns>
        public static async Task<string> GetAuthenticationHeader(string aadTenant, string aadClientAppId, string aadClientAppSecret, string aadResource)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(aadTenant, false);
            AuthenticationResult authenticationResult;

            if (string.IsNullOrEmpty(aadClientAppSecret))
            {
                Console.WriteLine("Please fill AAD application secret in ClientConfiguration if you choose authentication by the application.");
                throw new Exception("Failed OAuth by empty application secret.");
            }

            try
            {
                // OAuth through application by application id and application secret.
                var creadential = new ClientCredential(aadClientAppId, aadClientAppSecret);
                authenticationResult = await authenticationContext.AcquireTokenAsync(aadResource, creadential);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to authenticate with AAD by application with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                throw new Exception("Failed to authenticate with AAD by application.");
            }


            // Create and get JWT token
            return authenticationResult.CreateAuthorizationHeader();
        }
    }
}

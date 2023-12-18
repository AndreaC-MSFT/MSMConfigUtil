using Microsoft.PowerPlatform.Dataverse.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public class ConnectionOptionsFactory : IConnectionOptionsFactory
    {
        private const string defaultClientId = "51f81489-12ee-4a9e-aaae-a2591f45987d";
        public ConnectionOptions Create(Uri environmemntUrl, AuthTypes authType, string? clientId, SecureString? clientSecret)
        {
            var connectionOptions = new ConnectionOptions()
            {
                ServiceUri = environmemntUrl,
                RequireNewInstance = true,
                RedirectUri = new Uri("http://localhost"),
                LoginPrompt = Microsoft.PowerPlatform.Dataverse.Client.Auth.PromptBehavior.Auto
            };
            switch (authType)
            {
                case AuthTypes.Interactive:
                    connectionOptions.AuthenticationType = Microsoft.PowerPlatform.Dataverse.Client.AuthenticationType.OAuth;
                    connectionOptions.ClientId = clientId ?? defaultClientId;
                    break;
                case AuthTypes.ClientIdAndSecret:
                    connectionOptions.AuthenticationType = Microsoft.PowerPlatform.Dataverse.Client.AuthenticationType.ClientSecret;
                    if (string.IsNullOrEmpty(clientId) || clientSecret == null || clientSecret.Length == 0)
                        throw new ArgumentException("--client-id and --client-secret are required when --auth-type is ClientIdAndSecret");
                    connectionOptions.ClientId = clientId;
                    connectionOptions.ClientSecret = clientSecret.ToString();
                    break;
            }
            return connectionOptions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public class GlobalCLIOptions
    {
        public Uri sourceUri {  get; set; }
        public Uri destinationUri { get; set; }
        public AuthTypes AuthType {  get; set; }
        public string? ClientId { get; set; }
        public SecureString? ClientSecret { get; set; }
    }

    public class GlobalCLIOptionsBinder : BinderBase<GlobalCLIOptions>
    {
        private readonly Option<Uri> sourceUriOption;
        private readonly Option<Uri> destinationUriOption;
        private readonly Option<AuthTypes> authTypeOption;
        private readonly Option<string?> clientIdOption;
        private readonly Option<SecureString?> clientSecretOption;

        public GlobalCLIOptionsBinder(Option<Uri> sourceUriOption, Option<Uri> destinationUriOption, Option<AuthTypes> authTypeOption, Option<string?> clientIdOption, Option<SecureString?> clientSecretOption)
        {
            this.sourceUriOption = sourceUriOption;
            this.destinationUriOption = destinationUriOption;
            this.authTypeOption = authTypeOption;
            this.clientIdOption = clientIdOption;
            this.clientSecretOption = clientSecretOption;
        }
        protected override GlobalCLIOptions GetBoundValue(BindingContext bindingContext)
        {
            return new GlobalCLIOptions
            {
                AuthType = bindingContext.ParseResult.GetValueForOption(authTypeOption),
                ClientId = bindingContext.ParseResult.GetValueForOption(clientIdOption),
                ClientSecret = bindingContext.ParseResult.GetValueForOption(clientSecretOption),
                destinationUri = bindingContext.ParseResult.GetValueForOption(destinationUriOption),
                sourceUri = bindingContext.ParseResult.GetValueForOption(sourceUriOption)
            };
        }
    }
}

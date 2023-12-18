using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Security;

namespace MSMConfigUtil
{
    internal class CalculationModelControllerCLI
    {
        private ICalculationModelController _controller;

        public CalculationModelControllerCLI(ICalculationModelController controller)
        {
            _controller = controller;
        }

        public void ConfigureCommands(RootCommand rootCommand)
        {
            var sourceUriOption = new Option<Uri>(
                aliases: ["--source-environment-uri", "--source-uri", "--s"],
                description: "The Dataverse URI of the source environment")
            { IsRequired = true };
            rootCommand.AddGlobalOption(sourceUriOption);

            var destinationuriOption = new Option<Uri>(
                    aliases: ["--destination-environment-uri", "--destination-uri", "--d"],
                    description: "The Dataverse URI of the destination environment")
            { IsRequired = true };
            rootCommand.AddGlobalOption(destinationuriOption);

            var authTypeOption = new Option<AuthTypes>(
                    name: "--auth-type",
                    description: "The authentication type",
                    getDefaultValue: () => AuthTypes.Interactive)
            { IsRequired = true };
            rootCommand.AddGlobalOption(authTypeOption);

            var clientIdOption = new Option<string?>("--client-id", "The OAuth client ID (or App registration Id). Only used whith --auth-type ClientIdAndSecret.");
            rootCommand.AddGlobalOption(clientIdOption);

            var clientSecretOption = new Option<SecureString?>("--client-secret", "The client secret. Only used whith --auth-type ClientIdAndSecret.");
            rootCommand.AddGlobalOption(clientSecretOption);

            var migrateCalculationModelCommand = new Command("migrate-calculation-models", "Copies one or more calculation models from the source environment to the destination environment");
            var calculationModelNameOption = new Option<string?>(
                aliases: ["--calculation-model-name", "--name", "--n"],
                description: "The name of the calculation model");
            migrateCalculationModelCommand.AddOption(calculationModelNameOption);

            var migrateAllOption = new Option<bool>(
                name: "--all",
                description: "Migrate all custom calculation models found in the source system");
            migrateCalculationModelCommand.AddOption(migrateAllOption);

            var replaceExistingOption = new Option<bool>(
                name: "--replace-existing",
                description: "Replace the calculation model if it already exists in the destination environment",
                getDefaultValue: () => false)
            { IsRequired = true };
            migrateCalculationModelCommand.AddOption(replaceExistingOption);

            migrateCalculationModelCommand.SetHandler(_controller.MigrateCalculationModel,
                new GlobalCLIOptionsBinder(sourceUriOption, destinationuriOption, authTypeOption, clientIdOption, clientSecretOption),
                new MigrateModelsCLIOptionsBinder(calculationModelNameOption, migrateAllOption, replaceExistingOption));

            rootCommand.AddCommand(migrateCalculationModelCommand);
        }
    }
}

using Microsoft.Extensions.Logging;
using MSMConfigUtil.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public class CalculationModelMigrator : ICalculationModelMigrator
    {
        private readonly ICalculationModelReader destinationCalculationModelReader;
        private readonly IModelDefinitionConverter modelDefinitionConverter;
        private readonly ICalculationModelWriter calculationModelWriter;
        private readonly ICalculationModelReader sourceCalculationModelReader;
        private readonly IUserInterfaceHandler uiHandler;

        public CalculationModelMigrator(ICalculationModelReader sourceCalculationModelReader, ICalculationModelReader destinationCalculationModelReader, IModelDefinitionConverter modelDefinitionConverter, ICalculationModelWriter calculationModelWriter, IUserInterfaceHandler uiHandler)
        {
            this.sourceCalculationModelReader = sourceCalculationModelReader;
            this.destinationCalculationModelReader = destinationCalculationModelReader;
            this.modelDefinitionConverter = modelDefinitionConverter;
            this.calculationModelWriter = calculationModelWriter;
            this.uiHandler = uiHandler;
        }

        public void Migrate(bool replaceExisting)
        {
            uiHandler.ShowInformation("Listing all custom calculation model from source environment.");
            var sourceModels = sourceCalculationModelReader.GetAll();
            if (!sourceModels.Any())
                uiHandler.ShowError("Connot find any custom calculation models in the source environment. Default and demo calculation models cannot be migrated.");
            foreach (var sourceModel in sourceModels)
            {
                try
                {
                    Migrate(sourceModel, replaceExisting);
                }
                catch (SourceToDestinationIdConversionException convException)
                {
                    uiHandler.ShowError($"Cannot migrate calculation model '{sourceModel.Name}'. {convException.Message}");
                }
                catch (Exception ex)
                {
                    uiHandler.ShowError(ex, $"Cannot migrate calculation model '{sourceModel.Name}'. {ex.Message}");
                }
            }
        }

        public void Migrate(string calculationModelName, bool replaceExisting)
        {
            try
            {
                uiHandler.ShowInformation($"Retrieving calculation model '{calculationModelName}' from source environment.");
                CalculationModel sourceModel = sourceCalculationModelReader.Get(calculationModelName);

                uiHandler.ShowInformation("Starting migration.");
                Migrate(sourceModel, replaceExisting);
            }
            catch (SourceToDestinationIdConversionException convException)
            {
                uiHandler.ShowError(convException.Message);
            }
            catch (Exception ex)
            {
                uiHandler.ShowError(ex, $"Cannot migrate calculation model '{calculationModelName}'. {ex.Message}");
            }
        }

        public void Migrate(CalculationModel sourceModel, bool replaceExisting)
        {
            var modelIdAtDestination = destinationCalculationModelReader.GetId(sourceModel.Name);
            if (modelIdAtDestination.HasValue)
            {
                uiHandler.ShowInformation($"A calculation model with name '{sourceModel.Name}' already exists in the destination environment.");
                if (replaceExisting)
                {
                    uiHandler.ShowInformation("The existing model will be replaced.");
                    sourceModel.Id = modelIdAtDestination.Value.ToString();
                    sourceModel.JsonDefinition = modelDefinitionConverter.Convert(sourceModel.JsonDefinition);
                    calculationModelWriter.Update(sourceModel);
                    uiHandler.ShowInformation($"Calculation model '{sourceModel.Name}' migrated.");
                }
                else
                {
                    uiHandler.ShowWarning($"A calculation model with name '{sourceModel.Name}' already exists in the destination environment. To replace it please specify --replace-existing");
                }
            }
            else
            {
                sourceModel.JsonDefinition = modelDefinitionConverter.Convert(sourceModel.JsonDefinition);
                calculationModelWriter.Create(sourceModel);
                uiHandler.ShowInformation($"Calculation model '{sourceModel.Name}' migrated.");
            }
        }
    }
}

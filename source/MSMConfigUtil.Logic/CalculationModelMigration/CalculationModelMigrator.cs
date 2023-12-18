using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public class CalculationModelMigrator : ICalculationModelMigrator
    {
        private ICalculationModelReader sourceCalculationModelReader;
        private ICalculationModelReader destinationCalculationModelReader;
        private IModelDefinitionConverter modelDefinitionConverter;
        private ICalculationModelWriter calculationModelWriter;

        public CalculationModelMigrator(ICalculationModelReader sourceCalculationModelReader, ICalculationModelReader destinationCalculationModelReader, IModelDefinitionConverter modelDefinitionConverter, ICalculationModelWriter calculationModelWriter)
        {
            this.sourceCalculationModelReader = sourceCalculationModelReader;
            this.destinationCalculationModelReader = destinationCalculationModelReader;
            this.modelDefinitionConverter = modelDefinitionConverter;
            this.calculationModelWriter = calculationModelWriter;
        }

        public void Migrate(bool replaceExisting)
        {
            var sourceModels = sourceCalculationModelReader.GetAll();
            foreach (var sourceModel in sourceModels) Migrate(sourceModel, replaceExisting);
        }

        public void Migrate(string calculationModelName, bool replaceExisting)
        {
            CalculationModel sourceModel = sourceCalculationModelReader.Get(calculationModelName);
            Migrate(sourceModel, replaceExisting);
        }

        public void Migrate(CalculationModel sourceModel, bool replaceExisting)
        {
            var modelIdAtDestination = destinationCalculationModelReader.GetId(sourceModel.Name);
            if (modelIdAtDestination.HasValue)
            {
                if (replaceExisting)
                {
                    sourceModel.Id = modelIdAtDestination.Value.ToString();
                    sourceModel.JsonDefinition = modelDefinitionConverter.Convert(sourceModel.JsonDefinition);
                    calculationModelWriter.Update(sourceModel);
                }
                else
                {
                    //write message
                }
            }
            else
            {
                sourceModel.JsonDefinition = modelDefinitionConverter.Convert(sourceModel.JsonDefinition);
                calculationModelWriter.Create(sourceModel);
            }
        }
    }
}

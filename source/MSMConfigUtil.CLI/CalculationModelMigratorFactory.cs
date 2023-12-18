using MSMConfigUtil.Logic;
using MSMConfigUtil.Logic.CalculationModelMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public class CalculationModelMigratorFactory : ICalculationModelMigratorFactory
    {
        private readonly IOrganizationServiceFromCLIOptionsFactory organizationServiceFactory;
        private readonly IRetrieveResponseReader retrieveResponseReader;
        private readonly INodeConverterCollectionFactory nodeConverterCollectionFactory;
        private readonly IModelDefinitionHelper modelDefinitionHelper;

        public CalculationModelMigratorFactory(IOrganizationServiceFromCLIOptionsFactory organizationServiceFactory, IRetrieveResponseReader retrieveResponseReader, INodeConverterCollectionFactory nodeConverterCollectionFactory, IModelDefinitionHelper modelDefinitionHelper)
        {
            this.organizationServiceFactory = organizationServiceFactory;
            this.retrieveResponseReader = retrieveResponseReader;
            this.nodeConverterCollectionFactory = nodeConverterCollectionFactory;
            this.modelDefinitionHelper = modelDefinitionHelper;
        }

        public ICalculationModelMigrator Create(GlobalCLIOptions globalOptions)
        {
            var sourceOrganizationService = organizationServiceFactory.CreateSourceOrgService(globalOptions);
            var destinationOrganizationService = organizationServiceFactory.CreateDestinationOrgService(globalOptions);

            var sourceDataverseReader = new DataverseReader(sourceOrganizationService, retrieveResponseReader);
            var destinationDataverseReader = new DataverseReader(destinationOrganizationService, retrieveResponseReader);

            var sourceCalculationModelReader = new CalculationModelReader(sourceDataverseReader);
            var destinationCalculationModelReader = new CalculationModelReader(destinationDataverseReader);

            var idConverter = new IdConverter(sourceDataverseReader, destinationDataverseReader);
            var nodeConverterCollection = nodeConverterCollectionFactory.Create(idConverter, modelDefinitionHelper);

            var modelDefinitionConverter = new ModelDefinitionConverter(nodeConverterCollection, modelDefinitionHelper);

            var destinationDataverseWriter = new DataverseWriter(destinationOrganizationService);

            var calculationModelWriter = new CalculationModelWriter(destinationDataverseWriter);

            return new CalculationModelMigrator(sourceCalculationModelReader, destinationCalculationModelReader, modelDefinitionConverter, calculationModelWriter);
        }

    }
}

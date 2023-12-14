using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class CalculationModelMigrator
    {
        private ICalculationModelRepository sourceCalculationModelRepository;
        private ICalculationModelRepository destinationCalculationModelRepository;
        public CalculationModelMigrator(ICalculationModelRepository sourceCalculationModelRepository, ICalculationModelRepository destinationCalculationModelRepository)
        {
            this.sourceCalculationModelRepository = sourceCalculationModelRepository;
            this.destinationCalculationModelRepository = destinationCalculationModelRepository;
        }

        public void Migrate()
        {
            var sourceModels = sourceCalculationModelRepository.GetAll();
            foreach (var sourceModel in sourceModels) Migrate(sourceModel);
        }

        public void Migrate(CalculationModel sourceModel)
        {

        }
    }
}

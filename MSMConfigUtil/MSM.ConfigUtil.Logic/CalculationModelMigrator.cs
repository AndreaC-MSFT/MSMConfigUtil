using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class CalculationModelMigrator
    {
        private ICalculationModelReader sourceCalculationModelReader;
        private ICalculationModelReader destinationCalculationModelReader;
        public CalculationModelMigrator(ICalculationModelReader sourceCalculationModelReader, ICalculationModelReader destinationCalculationModelReader)
        {
            this.sourceCalculationModelReader = sourceCalculationModelReader;
            this.destinationCalculationModelReader = destinationCalculationModelReader;
        }

        public void Migrate()
        {
            var sourceModels = sourceCalculationModelReader.GetAll();
            foreach (var sourceModel in sourceModels) Migrate(sourceModel);
        }

        public void Migrate(CalculationModel sourceModel)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public interface ICalculationModelWriter
    {
        void Create(CalculationModel modelToWrite);
        void Update(CalculationModel modelToWrite);
    }
}

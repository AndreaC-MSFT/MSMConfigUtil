using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public interface ICalculationModelReader
    {
        Guid? GetId(string name);
        CalculationModel Get(string calculationModelName);
        IEnumerable<CalculationModel> GetAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public interface ICalculationModelReader
    {
        bool Exists(string name);
        CalculationModel Get(string calculationModelName);
        IEnumerable<CalculationModel> GetAll();
    }
}

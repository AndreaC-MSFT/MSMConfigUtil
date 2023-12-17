using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class CalculationModelReader : ICalculationModelReader
    {
        private readonly IDataverseReader dataverseReader;
        public CalculationModelReader(IDataverseReader dataverseReader)
        {
            this.dataverseReader = dataverseReader;
        }
        public IEnumerable<CalculationModel> GetAll()
        {
            return from c in dataverseReader.CreateQuery("msdyn_emissioncalculation")
                                  select new CalculationModel()
                                  {
                                      Id = c.Id.ToString(),
                                      Name = (string)c.Attributes["msdyn_name"],
                                      JsonDefinition = (string)c.Attributes["msdyn_calculationflowjson"]
                                  };

        }
    }
}

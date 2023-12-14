using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSM.ConfigUtil.Logic
{
    public class CalculationModelRepository : ICalculationModelRepository
    {
        private readonly IQueryProvider queryProvider;
        public CalculationModelRepository(IQueryProvider queryProvider)
        {
            this.queryProvider = queryProvider;
        }
        public IEnumerable<CalculationModel> GetAll()
        {
            return from c in queryProvider.CreateQuery("msdyn_emissioncalculation")
                                  select new CalculationModel()
                                  {
                                      Id = (string)c.Attributes["msdyn_emissioncalculationid"],
                                      Name = (string)c.Attributes["msdyn_name"],
                                      JsonDefinition = (string)c.Attributes["msdyn_calculationflowjson"]
                                  };
        }
    }
}

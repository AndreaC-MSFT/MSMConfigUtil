using Microsoft.Xrm.Sdk;
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
        private static string[] standardFields = { 
            CalculationModelsConstants.msdyn_emissioncalculationid,
            CalculationModelsConstants.msdyn_name,
            CalculationModelsConstants.msdyn_calculationflowjson };
        
        public CalculationModelReader(IDataverseReader dataverseReader)
        {
            this.dataverseReader = dataverseReader;
        }
        public IEnumerable<CalculationModel> GetAll()
        {
            return from c in dataverseReader.CreateQuery(CalculationModelsConstants.msdyn_emissioncalculation)
                   where (int)c[CalculationModelsConstants.msdyn_type] == CalculationModelsConstants.calculationModelType_Custom
                   select GetCalculationModelFromEntity(c);
        }


        public bool Exists(string name)
        {
            var id = dataverseReader.GetRowIdByKey(CalculationModelsConstants.msdyn_emissioncalculation, CalculationModelsConstants.msdyn_name, name);
            return id.HasValue;
        }

        public CalculationModel Get(string calculationModelName)
        {
            var result = from c in dataverseReader.CreateQuery(CalculationModelsConstants.msdyn_emissioncalculation)
                   where (string)c[CalculationModelsConstants.msdyn_name] == calculationModelName
                        && (int)c[CalculationModelsConstants.msdyn_type] == CalculationModelsConstants.calculationModelType_Custom
                   select GetCalculationModelFromEntity(c);
            return result.FirstOrDefault();
        }

        private CalculationModel GetCalculationModelFromEntity(Entity entity)
        {
            return new CalculationModel()
            {
                Id = entity.Id.ToString(),
                Name = (string)entity.Attributes[CalculationModelsConstants.msdyn_name],
                JsonDefinition = (string)entity.Attributes[CalculationModelsConstants.msdyn_calculationflowjson],
                AdditionalAttributes = entity.Attributes.Where(a => !standardFields.Contains(a.Key))
            };
        }
    }
}

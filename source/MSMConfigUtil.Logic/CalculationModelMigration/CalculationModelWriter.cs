using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic.CalculationModelMigration
{
    public class CalculationModelWriter : ICalculationModelWriter
    {
        private readonly IDataverseWriter dataverseWriter;

        public CalculationModelWriter(IDataverseWriter dataverseWriter)
        {
            this.dataverseWriter = dataverseWriter;
        }

        public void Create(CalculationModel modelToWrite)
        {
            Entity entity = CreateEntityFromModel(modelToWrite);
            dataverseWriter.Create(entity);
        }
        public void Update(CalculationModel modelToWrite)
        {
            Entity entity = CreateEntityFromModel(modelToWrite);
            entity.Id = new Guid(modelToWrite.Id);
            dataverseWriter.Update(entity);
        }

        private Entity CreateEntityFromModel(CalculationModel modelToWrite)
        {
            var entity = new Entity(CalculationModelsConstants.msdyn_emissioncalculation);
            entity[CalculationModelsConstants.msdyn_name] = modelToWrite.Name;
            entity[CalculationModelsConstants.msdyn_calculationflowjson] = modelToWrite.JsonDefinition;
            entity.Attributes.AddRange(modelToWrite.AdditionalAttributes.Where(a => !DataverseConstants.fieldsToExclude.Contains(a.Key)));
            return entity;
        }
    }
}

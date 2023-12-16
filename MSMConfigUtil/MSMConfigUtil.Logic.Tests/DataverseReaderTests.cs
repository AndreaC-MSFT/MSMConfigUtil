using Castle.Core.Internal;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MSM.ConfigUtil.Logic.Tests
{
    [TestFixture]
    public class DataverseReaderTests
    {
        private Mock<IOrganizationService> organizationServiceMock;
        private Mock<IRetrieveResponseReader> retrieveResponseReaderMock;
        private DataverseReader dataverseReader;

        [SetUp]
        public void Setup()
        {
            organizationServiceMock = new Mock<IOrganizationService>();
            retrieveResponseReaderMock = new Mock<IRetrieveResponseReader>();
            dataverseReader = new DataverseReader(organizationServiceMock.Object, retrieveResponseReaderMock.Object);
        }

        [Test]
        public void GetRowValueById_Should_Return_Value()
        {
            // Arrange
            string logicalTableName = "TableName";
            Guid rowId = Guid.NewGuid();
            string fieldName = "FieldName";
            var entity = new Entity(logicalTableName);
            entity.Attributes.Add(fieldName, "Value");
            organizationServiceMock.Setup(os => os.Retrieve(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<ColumnSet>())).Returns(entity);
            // Act
            var result = dataverseReader.GetRowValueById<string>(logicalTableName, rowId, fieldName);

            // Assert
            Assert.That(result, Is.EqualTo("Value"));
        }

        [Test]
        public void GetRowValueById_ShouldPassCorrectValuesToOrganizationService()
        {
            // Arrange
            string logicalTableName = "TableName";
            Guid rowId = Guid.NewGuid();
            string fieldName = "FieldName";
            var entity = new Entity(logicalTableName);
            entity.Attributes.Add(fieldName, "Value");
            organizationServiceMock.Setup(os => os.Retrieve(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<ColumnSet>())).Returns(entity);

            // Act
            var result = dataverseReader.GetRowValueById<string>(logicalTableName, rowId, fieldName);

            // Assert
            organizationServiceMock.Verify(os => os.Retrieve(logicalTableName, rowId, It.Is<ColumnSet>(cs => cs.Columns.Single() == fieldName)));
        }

        [Test]
        public void GetRowValueById_Should_Throw_Exception_When_Entity_Not_Found()
        {
            // Arrange
            string logicalTableName = "TableName";
            Guid rowId = Guid.NewGuid();
            string fieldName = "FieldName";
            organizationServiceMock.Setup(os => os.Retrieve(logicalTableName, rowId, It.IsAny<ColumnSet>())).Returns((Entity)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dataverseReader.GetRowValueById<string>(logicalTableName, rowId, fieldName));
        }

        [Test]
        public void GetRowValueById_Should_Throw_Exception_When_Conversion_Fails()
        {
            // Arrange
            string logicalTableName = "TableName";
            Guid rowId = Guid.NewGuid();
            string fieldName = "FieldName";
            var entity = new Entity(logicalTableName);
            entity.Attributes.Add(fieldName, 123);
            organizationServiceMock.Setup(os => os.Retrieve(logicalTableName, rowId, It.IsAny<ColumnSet>())).Returns(entity);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => dataverseReader.GetRowValueById<string>(logicalTableName, rowId, fieldName));
        }

        [Test]
        public void GetRowIdByKey_Should_Return_Id()
        {
            // Arrange
            string logicalTableName = "TableName";
            string keyFieldName = "KeyFieldName";
            string keyFieldValue = "KeyFieldValue";

            var returnedEntity = new Entity();
            returnedEntity.Id = Guid.NewGuid();

            var retrieveResponse = new RetrieveResponse();
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(retrieveResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(retrieveResponse)).Returns(returnedEntity);

            // Act
            var result = dataverseReader.GetRowIdByKey(logicalTableName, keyFieldName, keyFieldValue);

            // Assert
            Assert.That(result, Is.EqualTo(returnedEntity.Id));
        }

        [Test]
        public void GetRowIdByKey_Should_PassFiltersToOrganizationService()
        {
            // Arrange
            string logicalTableName = "TableName";
            string keyFieldName = "KeyFieldName";
            string keyFieldValue = "KeyFieldValue";

            var returnedEntity = new Entity();
            returnedEntity.Id = Guid.NewGuid();

            var retrieveResponse = new RetrieveResponse();
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(retrieveResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(retrieveResponse)).Returns(returnedEntity);

            // Act
            var result = dataverseReader.GetRowIdByKey(logicalTableName, keyFieldName, keyFieldValue);

            // Assert
            organizationServiceMock.Verify(os => os.Execute(It.Is<RetrieveRequest>(r =>
                r.Target.LogicalName == logicalTableName
                && r.Target.KeyAttributes.Single().Key == keyFieldName
                && (string)r.Target.KeyAttributes.Single().Value == keyFieldValue)));
        }

        [Test]
        public void GetRowIdByKey_Should_Throw_Exception_When_Entity_Not_Found()
        {
            // Arrange
            string logicalTableName = "TableName";
            string keyFieldName = "KeyFieldName";
            string keyFieldValue = "KeyFieldValue";
            RetrieveResponse nullResponse = null;
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(nullResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(nullResponse)).Returns((Entity)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dataverseReader.GetRowIdByKey(logicalTableName, keyFieldName, keyFieldValue));
            retrieveResponseReaderMock.Verify(m => m.GetEntity(nullResponse));
        }

        [Test]
        public void GetRowIdByKey_WithkeyFieldValueList_Should_Return_Id()
        {
            // Arrange
            string logicalTableName = "TableName";

            var keyFieldValueList = new KeyValuePair<string, object>[]
            {
                new("KeyFieldName1", "KeyValue1"),
                new("KeyFieldName2", 2)
            };

            var returnedEntity = new Entity();
            returnedEntity.Id = Guid.NewGuid();

            var retrieveResponse = new RetrieveResponse();
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(retrieveResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(retrieveResponse)).Returns(returnedEntity);

            // Act
            var result = dataverseReader.GetRowIdByKey(logicalTableName, keyFieldValueList);

            // Assert
            Assert.That(result, Is.EqualTo(returnedEntity.Id));
        }

        [Test]
        public void GetRowIdByKey_WithkeyFieldValueList_Should_PassFiltersToOrganizationService()
        {
            // Arrange
            string logicalTableName = "TableName";

            var keyFieldValueList = new KeyValuePair<string, object>[]
            {
                new("KeyFieldName1", "KeyValue1"),
                new("KeyFieldName2", 2)
            };

            var returnedEntity = new Entity();
            returnedEntity.Id = Guid.NewGuid();

            var retrieveResponse = new RetrieveResponse();
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(retrieveResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(retrieveResponse)).Returns(returnedEntity);

            // Act
            var result = dataverseReader.GetRowIdByKey(logicalTableName, keyFieldValueList);

            // Assert
            organizationServiceMock.Verify(os => os.Execute(It.Is<RetrieveRequest>(r =>
                r.Target.LogicalName == logicalTableName
                && r.Target.KeyAttributes.Count == 2
                && r.Target.KeyAttributes.Any(a => a.Key == keyFieldValueList[0].Key && a.Value == keyFieldValueList[0].Value)
                && r.Target.KeyAttributes.Any(a => a.Key == keyFieldValueList[1].Key && a.Value == keyFieldValueList[1].Value))));
        }

        [Test]
        public void GetRowIdByKey_WithkeyFieldValueList_Should_Throw_Exception_When_Entity_Not_Found()
        {
            // Arrange
            string logicalTableName = "TableName";

            var keyFieldValueList = new KeyValuePair<string, object>[]
            {
                new("KeyFieldName1", "KeyValue1"),
                new("KeyFieldName2", 2)
            };

            RetrieveResponse nullResponse = null;
            organizationServiceMock.Setup(os => os.Execute(It.IsAny<RetrieveRequest>())).Returns(nullResponse);
            retrieveResponseReaderMock.Setup(m => m.GetEntity(nullResponse)).Returns((Entity)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dataverseReader.GetRowIdByKey(logicalTableName, keyFieldValueList));
            retrieveResponseReaderMock.Verify(m => m.GetEntity(nullResponse));
        }

    }
}

using Moq;
using MSMConfigUtil.Logic.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace MSMConfigUtil.Logic.UnitTests
{
    [TestFixture]
    public class IdConverterTests
    {
        private Mock<IDataverseReader> sourceDataverseReaderMock;
        private Mock<IDataverseReader> destinationDataverseReaderMock;
        private IdConverter idConverter;

        [SetUp]
        public void Setup()
        {
            sourceDataverseReaderMock = new Mock<IDataverseReader>();
            destinationDataverseReaderMock = new Mock<IDataverseReader>();
            idConverter = new IdConverter(sourceDataverseReaderMock.Object, destinationDataverseReaderMock.Object);
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_Should_ReturnConvertedId()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            Assert.That(result, Is.EqualTo(expectedDestinationId));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_Should_ThrowExceptionWhenNoMatch()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = "ValueToMatch";
            Guid? expectedDestinationId = null;
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedDestinationId);

            // Act & Assert
            Assert.Throws<SourceToDestinationIdConversionException>(() => idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_Should_SearchSourceEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            sourceDataverseReaderMock.Verify(m => m.GetRowValueById<string>(logicalTableName, sourceRowId, fieldToMatchInDestination));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_Should_SearchDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            destinationDataverseReaderMock.Verify(m => m.GetRowIdByKey(logicalTableName, fieldToMatchInDestination, valueToMatchAtdestination));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentInt_Should_ReturnConvertedId()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = 123;
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<int>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<int>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            Assert.That(result, Is.EqualTo(expectedDestinationId));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentInt_Should_SearchSourceEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = 123;
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<int>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<int>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            sourceDataverseReaderMock.Verify(m => m.GetRowValueById<int>(logicalTableName, sourceRowId, fieldToMatchInDestination));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentInt_Should_SearchDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";
            var valueToMatchAtdestination = 123;
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<int>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<int>(logicalTableName, sourceRowId, fieldToMatchInDestination);

            // Assert
            destinationDataverseReaderMock.Verify(m => m.GetRowIdByKey(logicalTableName, fieldToMatchInDestination, valueToMatchAtdestination));
        }



        [Test]
        public void ConvertIdToDestinationEnvironmentString_WithAdditionalFilterCriteria_Should_ReturnConvertedId()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";

            var additionalFilterCriteria = new[]
            {
                new KeyValuePair<string, object>("FilterField1", "FilterValue1"),
                new KeyValuePair<string, object>("FilterField2", "FilterValue2")
            };

            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string,object>>>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination, additionalFilterCriteria);

            // Assert
            Assert.That(result, Is.EqualTo(expectedDestinationId));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_WithAdditionalFilterCriteria_Should_ThrowexceptionWhenNoMatch()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";

            var additionalFilterCriteria = new[]
            {
                new KeyValuePair<string, object>("FilterField1", "FilterValue1"),
                new KeyValuePair<string, object>("FilterField2", "FilterValue2")
            };

            var valueToMatchAtdestination = "ValueToMatch";
            Guid? expectedDestinationId = null;
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>())).Returns(expectedDestinationId);

            // Act & Assert
            Assert.Throws<SourceToDestinationIdConversionException>(() => idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination, additionalFilterCriteria));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_WithAdditionalFilterCriteria_Should_SearchSourceEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";

            var additionalFilterCriteria = new[]
            {
                new KeyValuePair<string, object>("FilterField1", "FilterValue1"),
                new KeyValuePair<string, object>("FilterField2", "FilterValue2")
            };

            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination, additionalFilterCriteria);

            // Assert
            sourceDataverseReaderMock.Verify(m => m.GetRowValueById<string>(logicalTableName, sourceRowId, fieldToMatchInDestination));
        }

        [Test]
        public void ConvertIdToDestinationEnvironmentString_WithAdditionalFilterCriteria_Should_SearchDestinationEnvironmentWithCorrectParameters()
        {
            // Arrange
            var logicalTableName = "TableName";
            var sourceRowId = Guid.NewGuid();
            var fieldToMatchInDestination = "Field";

            var additionalFilterCriteria = new[]
            {
                new KeyValuePair<string, object>("FilterField1", "FilterValue1"),
                new KeyValuePair<string, object>("FilterField2", "FilterValue2")
            };

            var valueToMatchAtdestination = "ValueToMatch";
            var expectedDestinationId = Guid.NewGuid();
            sourceDataverseReaderMock.Setup(m => m.GetRowValueById<string>(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(valueToMatchAtdestination);
            destinationDataverseReaderMock.Setup(m => m.GetRowIdByKey(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>())).Returns(expectedDestinationId);

            // Act
            var result = idConverter.ConvertIdToDestinationEnvironment<string>(logicalTableName, sourceRowId, fieldToMatchInDestination, additionalFilterCriteria);

            // Assert
            Func<IEnumerable<KeyValuePair<string, object>>, bool> filterVerificationExpression = (
                filterCriteria => filterCriteria.Count() == 3
                    && filterCriteria.Any(f => f.Key == fieldToMatchInDestination && (string)f.Value == valueToMatchAtdestination)
                    && filterCriteria.Any(f => f.Key == additionalFilterCriteria[0].Key && f.Value == additionalFilterCriteria[0].Value)
                    && filterCriteria.Any(f => f.Key == additionalFilterCriteria[1].Key && f.Value == additionalFilterCriteria[1].Value));
            destinationDataverseReaderMock.Verify(m => m.GetRowIdByKey(logicalTableName, It.Is<IEnumerable<KeyValuePair<string, object>>>(f => filterVerificationExpression(f))));
        }

    }
}

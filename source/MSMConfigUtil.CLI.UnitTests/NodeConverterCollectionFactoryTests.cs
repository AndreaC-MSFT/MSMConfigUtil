using Moq;
using MSMConfigUtil.Logic;
using MSMConfigUtil.Logic.CalculationModelMigration;
using MSMConfigUtil.Logic.CalculationModelMigration.NodesConversion;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MSMConfigUtil.CLI.Tests
{
    [TestFixture]
    public class NodeConverterCollectionFactoryTests
    {
        private Mock<IIdConverter> _idConverterMock;
        private Mock<IModelDefinitionHelper> _modelDefinitionHelperMock;
        private NodeConverterCollectionFactory _nodeConverterCollectionFactory;

        [SetUp]
        public void Setup()
        {
            _idConverterMock = new Mock<IIdConverter>();
            _modelDefinitionHelperMock = new Mock<IModelDefinitionHelper>();
            _nodeConverterCollectionFactory = new NodeConverterCollectionFactory();
        }

        [Test]
        public void Create_ReturnsConverters()
        {
            // Act
            var converters = _nodeConverterCollectionFactory.Create(_idConverterMock.Object, _modelDefinitionHelperMock.Object);

            // Assert
            var filterConverters = converters.Where(c => c.GetType().Namespace.StartsWith("MSMConfigUtil.CLI.Tests"));
            Assert.That(filterConverters.Count(), Is.EqualTo(3));
            Assert.That(filterConverters.Any(c => c.GetType() == typeof(TestNodeConverter1)), Is.True);
            Assert.That(filterConverters.Any(c => c.GetType() == typeof(TestNodeConverter2)), Is.True);
            Assert.That(filterConverters.Any(c => c.GetType() == typeof(TestNodeConverter3)), Is.True);
        }
    }

    public class TestNodeConverter1 : IModelDefinitionNodeConverter
    {
        public TestNodeConverter1(IIdConverter idConverter)
        {
            Assert.That(idConverter, Is.Not.Null);
        }

        public IEnumerable<string> SupportedActionTypes => throw new NotImplementedException();

        public bool SupportsAnyNode => throw new NotImplementedException();

        public void InspectAndConvert(JObject node)
        {
            throw new NotImplementedException();
        }
    }

    public class TestNodeConverter2 : IModelDefinitionNodeConverter
    {
        public TestNodeConverter2(IModelDefinitionHelper modelDefinitionHelper)
        {
            Assert.That(modelDefinitionHelper, Is.Not.Null);
        }

        public IEnumerable<string> SupportedActionTypes => throw new NotImplementedException();

        public bool SupportsAnyNode => throw new NotImplementedException();

        public void InspectAndConvert(JObject node)
        {
            throw new NotImplementedException();
        }
    }

    public class TestNodeConverter3 : IModelDefinitionNodeConverter
    {
        public TestNodeConverter3(IIdConverter idConverter, IModelDefinitionHelper modelDefinitionHelper)
        {
            Assert.That(idConverter, Is.Not.Null);
            Assert.That(modelDefinitionHelper, Is.Not.Null);
        }

        public IEnumerable<string> SupportedActionTypes => throw new NotImplementedException();

        public bool SupportsAnyNode => throw new NotImplementedException();

        public void InspectAndConvert(JObject node)
        {
            throw new NotImplementedException();
        }
    }
}

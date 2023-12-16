using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System;

namespace MSM.ConfigUtil.Logic.Tests
{
    [TestFixture]
    public class ModelDefinitionHelperTests
    {
        private ModelDefinitionHelper _modelDefinitionHelper;

        [SetUp]
        public void Setup()
        {
            _modelDefinitionHelper = new ModelDefinitionHelper();
        }

        [Test]
        public void GetActionType_Should_Return_ActionType()
        {
            // Arrange
            JObject node = JObject.Parse(@"
                {
                    ""prop1"": ""value1"",
                    ""actionType"": ""ActionA"",
                    ""prop2"": ""value2"",
                    ""subObject"": { ""actionType"" : ""SomeOtherAction"" }
                }");

            // Act
            string result = _modelDefinitionHelper.GetActionType(node);

            // Assert
            Assert.That(result, Is.EqualTo("ActionA"));
        }

        [Test]
        public void GetActionType_Should_ReturnEmptyStringWhenThereIsNoActionType()
        {
            // Arrange
            JObject node = JObject.Parse(@"
                {
                    ""prop1"": ""value1"",
                    ""prop2"": ""value2"",
                    ""subObject"": { ""actionType"" : ""SomeOtherAction"" }
                }");

            // Act
            string result = _modelDefinitionHelper.GetActionType(node);

            // Assert
            Assert.That(result, Is.Empty);
        }


        [Test]
        public void GetPropertyValue_Should_Return_PropertyValue()
        {
            // Arrange
            JObject node = JObject.Parse(@"
                {
                    ""prop1"": ""value1"",
                    ""prop2"": ""value2"",
                    ""subObject"": { ""prop2"" : ""otherValue"" }
                }");
            // Act
            string? result = _modelDefinitionHelper.GetPropertyValue(node, "prop2");

            // Assert
            Assert.That(result, Is.EqualTo("value2"));
        }

        [Test]
        public void GetPropertyValue_Should_ReturnNullWhenPropertyDoesNotExist()
        {
            // Arrange
            JObject node = JObject.Parse(@"
                {
                    ""prop1"": ""value1"",
                    ""prop2"": ""value2"",
                    ""subObject"": { ""inexistingProp"" : ""otherValue"" }
                }");
            // Act
            string? result = _modelDefinitionHelper.GetPropertyValue(node, "inexistingProp");

            // Assert
            Assert.That(result, Is.Null);
        }


        [Test]
        public void SetPropertyValue_Should_Replace_PropertyValue()
        {
            // Arrange
            JObject node = JObject.Parse(@"
                {
                    ""prop1"": ""value1"",
                    ""prop2"": ""oldValue"",
                    ""subObject"": { ""prop2"" : ""otherValue"" }
                }");
            // Act
            _modelDefinitionHelper.SetPropertyValue(node, "prop2", "NewValue");

            // Assert
            Assert.That(node["prop2"].ToString(), Is.EqualTo("NewValue"));
        }

        [Test]
        public void IsGuid_Should_Return_True_When_Value_Is_Valid_Guid()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            bool result = _modelDefinitionHelper.IsGuid(value);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsGuid_Should_Return_False_When_Value_Is_Not_Valid_Guid()
        {
            // Arrange
            string value = "InvalidGuid";

            // Act
            bool result = _modelDefinitionHelper.IsGuid(value);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}

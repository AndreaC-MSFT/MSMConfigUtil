using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Xml.Linq;

namespace MSM.ConfigUtil.Logic.Tests
{
    [TestFixture]
    public class ModelDefinitionConverterTests
    {
        private Mock<IModelDefinitionNodeConverter> mockNodeConverterActionA;
        private Mock<IModelDefinitionNodeConverter> mockNodeConverterActionB;
        private Mock<IModelDefinitionNodeConverter> mockNodeConverterActionsCandD;
        private Mock<IModelDefinitionNodeConverter> mockNodeConverterAllActions;
        private Mock<IModelDefinitionHelper> mockModelDefinitionHelper;
        private ModelDefinitionConverter modelDefConverter;

        [SetUp]
        public void Setup()
        {
            mockNodeConverterActionA = new Mock<IModelDefinitionNodeConverter>();
            mockNodeConverterActionA.SetupGet(m => m.SupportedActionTypes).Returns(new[] { "ActionA" });
            mockNodeConverterActionA.SetupGet(m => m.SupportsAnyNode).Returns(false);

            mockNodeConverterActionB = new Mock<IModelDefinitionNodeConverter>();
            mockNodeConverterActionB.SetupGet(m => m.SupportedActionTypes).Returns(new[] { "ActionB" });
            mockNodeConverterActionB.SetupGet(m => m.SupportsAnyNode).Returns(false);

            mockNodeConverterActionsCandD = new Mock<IModelDefinitionNodeConverter>();
            mockNodeConverterActionsCandD.SetupGet(m => m.SupportedActionTypes).Returns(new[] { "ActionC", "ActionD" });
            mockNodeConverterActionsCandD.SetupGet(m => m.SupportsAnyNode).Returns(false);
            
            mockNodeConverterAllActions = new Mock<IModelDefinitionNodeConverter>();
            mockNodeConverterAllActions.SetupGet(m => m.SupportsAnyNode).Returns(true);

            mockModelDefinitionHelper = new Mock<IModelDefinitionHelper>();


            mockModelDefinitionHelper.Setup(m => m.GetActionType(It.IsAny<JObject>())).Returns("SomethingElse");

            foreach (var actionType in new[] { "ActionA", "ActionB", "ActionC", "ActionD", })
            {
                mockModelDefinitionHelper.Setup(m => m.GetActionType(It.Is<JObject>(o => o.Properties().Any(
                    p => p.Name == "actionType" && p.Value.ToString() == actionType)))).Returns(actionType);
            }

            modelDefConverter = new ModelDefinitionConverter(new[] {
                    mockNodeConverterActionA.Object,
                    mockNodeConverterActionB.Object,
                    mockNodeConverterActionsCandD.Object,
                    mockNodeConverterAllActions.Object },
                mockModelDefinitionHelper.Object);
        }

        [Test]
        public void Convert_Should_MatchNodeConverterByActionTypeOnJsonNode()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Action A node"",
                        ""actionType"": ""ActionA"",
                        ""nextAction"": {
                            ""name"": ""Report node"",
                            ""actionType"": ""Report""
                        }
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionA.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Once);
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionsCandD.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
        }

        [Test]
        public void Convert_Should_MatchNodeConverterByActionTypeOnJsonSubNode()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextAction"": {
                            ""name"": ""Action B node"",
                            ""actionType"": ""ActionB""
                        }
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionA.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Once);
            mockNodeConverterActionsCandD.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
        }

        [Test]
        public void Convert_Should_MatchNodeConverterSupportingMultipleActionTypes()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Action C node"",
                        ""actionType"": ""ActionC"",
                        ""nextAction"": {
                            ""name"": ""Action D node"",
                            ""actionType"": ""ActionD""
                        }
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionA.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionsCandD.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Exactly(2));
        }

        [Test]
        public void Convert_Should_MatchNodeConverterSupportingAllActions()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextAction"": {
                            ""name"": ""Something else"",
                            ""actionType"": ""SomethingElse""
                        }
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterAllActions.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Exactly(3));
            mockNodeConverterActionA.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionsCandD.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
        }

        [Test]
        public void Convert_Should_PassPropertiesToNodeConverters()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextAction"": {
                            ""name"": ""Action B node"",
                            ""actionType"": ""ActionB""
                        }
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(
                It.Is<JObject>(o => o.Properties().Single(p => p.Name == "name").Value.ToString() == "Action B node")),
                Times.Once);
        }

        [Test]
        public void Convert_Should_IngnoreCaseInActionTypeMatching()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextAction"": {
                            ""name"": ""Action B node"",
                            ""actionType"": ""ACTIONb""
                        }
                    }
                }";
            mockModelDefinitionHelper.Setup(m => m.GetActionType(It.Is<JObject>(o => o.Properties().Any(
                p => p.Name == "actionType" && p.Value.ToString() == "ACTIONb")))).Returns("ACTIONb");

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionA.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Once);
            mockNodeConverterActionsCandD.Verify(c => c.InspectAndConvert(It.IsAny<JObject>()), Times.Never);
        }

        [Test]
        public void Convert_Should_ReturnConvertedJson()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextAction"": {
                            ""name"": ""Action B node"",
                            ""actionType"": ""ActionB""
                        }
                    }
                }";
            mockNodeConverterActionB.Setup(m => m.InspectAndConvert(It.IsAny<JObject>())).Callback<JObject>(o =>
            {
                foreach (var prop in o.Properties().Where(p => p.Name == "name"))
                    prop.Value = "ConvertedValue";
            });

            // Act
            var result = modelDefConverter.Convert(json);

            // Assert
            StringAssert.Contains("\"name\": \"ConvertedValue\"", result);
        }

        [Test]
        public void Convert_Should_WorksWithObjectArrays()
        {
            // Arrange
            var json = @"
                {
                    ""rootAction"": {
                        ""name"": ""Something else"",
                        ""actionType"": ""SomethingElse"",
                        ""nextActions"": [
                            {
                                ""name"": ""Another action"",
                                ""actionType"": ""ActionSomething""
                            },
                            {
                                ""name"": ""Action B node"",
                                ""actionType"": ""ActionB""
                            }
                        ]
                    }
                }";

            // Act
            modelDefConverter.Convert(json);

            // Assert
            mockNodeConverterActionB.Verify(c => c.InspectAndConvert(
                It.Is<JObject>(o => o.Properties().Single(p => p.Name == "name").Value.ToString() == "Action B node")),
                Times.Once);
        }
    }
}

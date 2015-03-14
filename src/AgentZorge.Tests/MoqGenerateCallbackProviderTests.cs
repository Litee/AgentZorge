using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqGenerateCallbackProviderTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestCase("TestVoidCallbackWithoutParameters.cs")]
        [TestCase("TestVoidCallbackWithSingleParameter.cs")]
        [TestCase("TestVoidCallbackWithMultipleParameters.cs")]
        [TestCase("TestVoidCallbackWithMultipleParametersAndMultipleChoices.cs")]
        [TestCase("TestVoidCallbackWithMultipleParametersAndMultipleChoices_2.cs")]
        [TestCase("TestVoidCallbackWithSingleParameterNonVoid.cs")]
        public void Test(string fileName)
        {
            DoTestFiles(fileName);
        }

        protected override bool ExecuteAction
        {
            get
            {
                return false;
            }
        }
    }
}
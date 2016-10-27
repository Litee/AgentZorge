#if RESHARPER9
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
#endif
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
        [TestCase("TestVoidCallbackWithSingleGenericParameter.cs")]
        [TestCase("TestVoidCallbackWithSingleGenericParameter_2.cs")]
        public void RunAll_MoqGenerateCallbackProviderTests(string fileName)
        {
            DoTestFiles(fileName);
        }

#if RESHARPER9
        protected override CodeCompletionTestType TestType
        {
            get { return CodeCompletionTestType.List; }
        }
#endif
    }
}
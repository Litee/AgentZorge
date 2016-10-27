#if RESHARPER9
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
#endif
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqGenerateItIsAnyProviderTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestCase("TestParameterSuggestionForFieldMock.cs")]
        [TestCase("TestParameterSuggestionForFieldMockSecondParameter.cs")]
        [TestCase("TestParameterSuggestionForFieldMockWithMultipleMethodOverloads.cs")]
        [TestCase("TestParameterSuggestionForFieldMockWithMultipleParameters.cs")]
        [TestCase("TestParameterSuggestionForFieldMockWithMultipleParameters_2.cs")]
        [TestCase("TestParameterSuggestionForLocalMock.cs")]
        public void RunAll_MoqSetupMethodParameterCodeCompletionProviderTests(string fileName)
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
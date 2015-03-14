using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqSuggestMocksForArgumentsTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestCase("SuggestLocalMockForArgument.cs")]
        [TestCase("SuggestFieldMockForArgument.cs")]
        [TestCase("SuggestParamMockForArgument.cs")]
        [TestCase("SuggestLocalMockForSecondArgument.cs")]
        [TestCase("SuggestLocalMockForArgumentTwoCandidates.cs")]

        public void RunAll_MoqSuggestMocksForParametersTests(string fileName)
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
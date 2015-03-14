using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqSuggestMocksForParametersTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestCase("SuggestLocalMockForParameter.cs")]

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
using JetBrains.ReSharper.Daemon.CSharp;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqIncompatibleCallbackParametersAnalysisTests : CSharpHighlightingTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestReferences("System.Core.dll")]
        [TestCase("TestCallbackWithRightParameters.cs")]

        [TestCase("TestCallbackWithLessParametersThanRequired.cs")]
        [TestCase("TestCallbackWithWrongParameterTypes.cs")]
        [TestCase("TestCallbackWithoutParameters.cs")]
        [TestCase("TestCallbackWithRightParametersIncludingAction.cs")]
        public void Test(string testFile)
        {
            DoTestSolution(testFile);
        }
    }
}

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
        [TestCase("TestCallbackWithLessParametersThanRequired.cs")]
        [TestCase("TestCallbackWithLessParametersThanRequiredAfterReturns.cs")]
        [TestCase("TestCallbackWithRightParameters.cs")]
        [TestCase("TestCallbackWithRightParametersAfterReturns.cs")]
        [TestCase("TestCallbackWithRightParametersAfterReturns_2.cs")]

        [TestCase("TestCallbackWithLessParametersThanRequired.cs")]
        [TestCase("TestCallbackWithWrongParameterTypes.cs")]
        [TestCase("TestCallbackWithWrongParameterTypes_2.cs")]
        [TestCase("TestCallbackWithoutParameters.cs")]
        [TestCase("TestCallbackWithRightParametersIncludingParams.cs")]
        public void RunAll_MoqIncompatibleCallbackParametersAnalysisTests(string testFile)
        {
            DoTestSolution(testFile);
        }
    }
}

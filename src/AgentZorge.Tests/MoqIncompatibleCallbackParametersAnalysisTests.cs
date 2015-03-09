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
        [TestCase("TestCallbackWithRightParameters.cs")]
        [TestCase("TestCallbackWithWrongParameterTypes.cs")]
        [TestCase("TestCallbackWithoutParameters.cs")]
        public void Test(string testFile)
        {
            DoTestFiles(testFile);
        }
    }
}

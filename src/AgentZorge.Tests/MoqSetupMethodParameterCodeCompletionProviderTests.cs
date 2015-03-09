using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqSetupMethodParameterCodeCompletionProviderTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("../Moq.dll")]
        [TestCase("TestCallParameter01.cs")]
        [TestCase("TestCallParameter02.cs")]
        [TestCase("TestCallParameter03.cs")]
        [TestCase("TestCallParameter04.cs")]

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
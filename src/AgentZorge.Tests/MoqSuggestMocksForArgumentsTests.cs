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
        [TestCase("SuggestConstructorArgumentExistingFieldMock.cs")]
        [TestCase("SuggestConstructorArgumentExistingLocalMock.cs")]
        [TestCase("SuggestConstructorArgumentExistingLocalMock_2.cs")]
        [TestCase("SuggestConstructorArgumentExistingParamMock.cs")]
        [TestCase("SuggestConstructorArgumentNewMockOnly.cs")]
        [TestCase("SuggestConstructorArgumentNewMockWithGeneralizedParameterOnly.cs")]
        [TestCase("SuggestConstructorArgumentNewMockWithGeneralizedParameterOnly_2.cs")]
        [TestCase("SuggestConstructorArgumentNonMockable.cs")]
        [TestCase("SuggestConstructorArgumentTwoExistingLocalMocks.cs")]
        [TestCase("SuggestMethodArgumentNewMockOnly.cs")]
        [TestCase("SuggestMethodArgumentManyExistingMocks.cs")]
        public void RunAll_MoqSuggestMocksForParametersTests(string fileName)
        {
            DoTestFiles(fileName);
        }

        [Test]
        [TestNetFramework4]
        [TestCase("SuggestConstructorArgumentWithoutMoqReferenced.cs")]
        public void RunAll_MoqSuggestMocksForParametersTests_NoMoq(string fileName)
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
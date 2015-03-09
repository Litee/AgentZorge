using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TextControl;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [TestFixture]
    public class MoqVarNameCodeCompletionProviderTests : CodeCompletionTestBase
    {
        [Test]
        [TestNetFramework4]
        [TestReferences("Moq.dll")]
        [TestCase("TestLocalVar01.cs")]
        [TestCase("TestLocalVar02.cs")]
        [TestCase("TestLocalVar03.cs")]
        [TestCase("TestLocalVar04.cs")]
        [TestCase("TestLocalVar05.cs")]
        [TestCase("TestLocalVar06.cs")]
        [TestCase("TestLocalVar07.cs")]
        [TestCase("TestLocalVar08.cs")]
        [TestCase("TestPrivateInstanceField01.cs")]
        [TestCase("TestPrivateInstanceField02.cs")]
        [TestCase("TestPrivateInstanceField03.cs")]
        [TestCase("TestPrivateInstanceField04.cs")]
        [TestCase("TestPrivateInstanceField05.cs")]
        [TestCase("TestPrivateInstanceField06.cs")]
        [TestCase("TestPrivateStaticField01.cs")]
        [TestCase("TestPrivateStaticReadonlyField01.cs")]
        [TestCase("TestPublicInstanceField01.cs")]
        [TestCase("TestPublicStaticField01.cs")]
        [TestCase("TestPublicStaticReadonlyField01.cs")]

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

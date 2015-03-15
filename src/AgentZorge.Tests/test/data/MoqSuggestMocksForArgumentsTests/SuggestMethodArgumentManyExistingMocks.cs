// ${COMPLETE_ITEM:It.Is}
// ${CODE_COMPLETION_TYPE:Smart}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;

namespace MoqExperiments
{
    [TestFixture]
    public class MyTest
    {
    	Mock<ITestService> testServiceMockField;
        [Test]
        public void Test(Mock<ITestService> testServiceMockParam)
        {
	     Mock<ITestService> testServiceMockLocal;
	     new TestClass().Initialize({caret}
        }
    }

    public interface ITestService
    {
    }

    public class TestClass
    {
        private ITestService _testService;

        public void Initialize(ITestService testService)
        {
            _testService = testService;
        }
    }
}
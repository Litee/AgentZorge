// ${COMPLETE_ITEM:It.Is}
// ${CODE_COMPLETION_TYPE:Smart}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;
using Moq;

namespace MoqExperiments
{
    [TestFixture]
    public class MyTest
    {

        [Test]
        public void Test()
        {
	     Mock<ITestService> testServiceMock;
	     Mock<ITestServiceBase> testServiceBaseMock;
	     new TestClass({caret}
        }
    }

    public interface ITestService : ITestServiceBase
    {
    }

    public interface ITestServiceBase
    {
        void Call(string s);
    }

    public class TestClass
    {
        private ITestServiceBase _testService;

        public TestClass(ITestServiceBase testService)
        {
            _testService = testService;
        }

        public void Call()
        {
            _testService.Call("a");
        }
    }
}
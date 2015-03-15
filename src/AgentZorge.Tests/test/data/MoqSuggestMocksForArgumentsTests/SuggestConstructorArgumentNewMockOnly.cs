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

        [Test]
        public void Test()
        {
	     new TestClass({caret}
        }
    }

    public interface ITestService
    {
    }

    public class TestClass
    {
        private ITestService _testService;

        public TestClass(ITestService testService)
        {
            _testService = testService;
        }
    }
}
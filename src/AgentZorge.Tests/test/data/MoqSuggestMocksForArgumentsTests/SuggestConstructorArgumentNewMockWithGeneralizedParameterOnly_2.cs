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
	     new TestClass<ITestService>({caret}
        }
    }

    public interface ITestService
    {
    }

    public class TestClass<T>
    {
        private T _testService;

        public TestClass(IEnumerable<T> testService)
        {
            _testService = testService;
        }
    }
}
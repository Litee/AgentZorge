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
            Mock<ITestService> _testServiceMock;
            _testServiceMock.Setup(x => x.Call({caret}
        }
    }

    public interface ITestService
    {
        void Call(string s);
    }
}
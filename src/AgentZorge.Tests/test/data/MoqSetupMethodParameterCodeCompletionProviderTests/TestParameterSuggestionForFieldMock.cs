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
        private Mock<ITestService> _testServiceMock;

        [Test]
        public void Test()
        {
            _testServiceMock.Setup(x => x.Call({caret}
        }
    }

    public interface ITestService
    {
        void Call(string s);
    }
}
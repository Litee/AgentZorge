using Moq;
using System.Collections.Generic;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(Is.Any<IEnumerable<string>>(), 0)).Callback({caret}
        }
    }

    public interface ITestService
    {
        void Call(IEnumerable<string> e, int i);
    }
}
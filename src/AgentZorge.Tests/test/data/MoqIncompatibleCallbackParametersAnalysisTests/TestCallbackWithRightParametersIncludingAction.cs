using System;
using Moq;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Action<string, byte[]>>())).Callback((string s, int i, Action<string, byte[]> a) => { });
        }
    }

    public interface ITestService
    {
        void Call(string s, int i, Action<string, byte[]> a);
    }
}
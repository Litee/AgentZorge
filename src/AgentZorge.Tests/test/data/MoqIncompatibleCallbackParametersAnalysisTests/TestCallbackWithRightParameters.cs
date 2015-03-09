using Moq;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(It.IsAny<string>(), It.IsAny<int>())).Callback((string s, int i) => { });
        }
    }

    public interface ITestService
    {
        void Call(string s, int i);
    }
}
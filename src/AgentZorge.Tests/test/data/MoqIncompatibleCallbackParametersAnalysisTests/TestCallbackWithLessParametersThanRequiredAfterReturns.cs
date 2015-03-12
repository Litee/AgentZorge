using Moq;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(It.IsAny<string>(), It.IsAny<int>())).Returns("").Callback((string s) => { });
        }
    }

    public interface ITestService
    {
        string Call(string s, int i);
    }
}
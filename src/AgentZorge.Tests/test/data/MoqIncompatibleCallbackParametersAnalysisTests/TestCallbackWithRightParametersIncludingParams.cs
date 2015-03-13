using Moq;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(It.IsAny<string>())).Callback((string[] s) => { });
        }
    }

    public interface ITestService
    {
        void Call(params string[] s);
    }
}
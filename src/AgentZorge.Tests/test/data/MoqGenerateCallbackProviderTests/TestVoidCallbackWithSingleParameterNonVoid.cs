using Moq;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call("")).Callback({caret}
        }
    }

    public interface ITestService
    {
        int Call(string s);
    }
}
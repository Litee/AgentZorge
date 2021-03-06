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
        void Call(string s);

        void Call(string s, int i);
    }
}
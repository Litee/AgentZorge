using Moq;
using System.Collections.Generic;

namespace TestNamespace
{
    public class MyTest
    {
        public void TestMethod(Mock<ITestService> testServiceMock)
        {
            testServiceMock.Setup(x => x.Call(It.IsAny<IEnumerable<string>>(), It.IsAny<int>())).Callback((string s, double d) => { });
        }
    }

    public interface ITestService
    {
        void Call<T>(IEnumerable<T> e, int i);
    }
}
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

namespace AgentZorge.Tests
{
    [ZoneDefinition]
    public interface IAgentZorgeTestZone : ITestsZone, IRequire<PsiFeatureTestZone>
    {}

    [SetUpFixture]
    public class TestEnvironmentAssembly : ExtensionTestEnvironmentAssembly<IAgentZorgeTestZone>
    {
    }
}
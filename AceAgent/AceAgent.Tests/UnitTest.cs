using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using AceAgent.BaseServiceInterface;
using AceAgent.BaseServiceModel;

namespace AceAgent.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<BaseServices>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Can_call_MyServices()
        {
            var service = appHost.Container.Resolve<BaseServiceIsAlive>();

            var response = (IsAliveResponse)service.Any(new BaseServiceIsAlive {});

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}

using Funq;
using ServiceStack;
using ServiceStack.Testing;
using Ace.AceService.BaseServiceInterface;
using Ace.AceService.BaseServiceModel;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Ace.AceService.BaseService.UnitTests
{
    class UnitTestingAppHost : AppSelfHostBase
    {
        public UnitTestingAppHost() : base(nameof(UnitTest), typeof(BaseServices).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }
    public class Fixture
    {
        public const string BaseUri = "http://localhost:2000/";
        public readonly ServiceStackHost unitTestingAppHost;
        #region MOQs
        // a MOQ for the async web calls used for Term1
        //public Mock<IWebGet> mockTerm1;
        #endregion

        //public ISSDataConcrete iSSData;

        public Fixture()
        {
            unitTestingAppHost = new UnitTestingAppHost()
    .Init()
    .Start(BaseUri);
            /*
            mockTerm1 = new Mock<ITcp>();
            mockTerm1.Setup(webGet => webGet.AsyncWebGet<double>("A"))
                .Callback(() => Task.Delay(new TimeSpan(0, 0, 1)))
                .ReturnsAsync(100.0);
            mockTerm1.Setup(webGet => webGet.AsyncWebGet<double>("B"))
                .Callback(() => Task.Delay(new TimeSpan(0, 0, 1)))
                .ReturnsAsync(200.0);
                */
        }
    } 

    public class UnitTest : IClassFixture<Fixture>
    {
        protected Fixture _fixture;
        readonly ITestOutputHelper output;
        public UnitTest(ITestOutputHelper output, Fixture fixture)
        {
            this.output = output;
            this._fixture = fixture;
        }

        public IServiceClient CreateClient() => new JsonServiceClient(Fixture.BaseUri);


        [Fact]
        public void Can_call_BaseServiceIsAlive()
        {
            var service = _fixture.unitTestingAppHost.Container.Resolve<BaseServices>();

            var response = (IsAliveResponse)service.Any(new BaseServiceIsAlive());
            response.Result.Should().Be("Hello!");
        }
    }
}

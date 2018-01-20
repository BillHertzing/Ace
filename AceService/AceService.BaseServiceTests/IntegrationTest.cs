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

namespace Ace.AceService.BaseService.IntegrationTests
{
    class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(IntegrationTest), typeof(BaseServices).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }
    public class Fixture
    {
        public const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;
        #region MOQs
        // a MOQ for the async web calls used for Term1
        //public Mock<IWebGet> mockTerm1;
        #endregion

        //public ISSDataConcrete iSSData;

        public Fixture()
        {
            appHost = new AppHost()
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

    public class IntegrationTest : IClassFixture<Fixture>
    {
        protected Fixture _fixture;
        readonly ITestOutputHelper output;
        public IntegrationTest(ITestOutputHelper output, Fixture fixture)
        {
            this.output = output;
            this._fixture = fixture;
        }

        public IServiceClient CreateClient() => new JsonServiceClient(Fixture.BaseUri);

        [Fact]
        public void Can_call_Hello_Service()
        {
            var client = CreateClient();

            var response = client.Get(new BaseServiceIsAlive {  });

            response.Result.Should().Be("Hello!");

        }
    }
}
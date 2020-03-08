using System;
using System.Net.Http;
using Http;
using Http.Abstract;
using Moq;
using Xunit;

namespace HttpTest {
    public class UnitTest1 {

        public UnitTest1() {

        }

        [Fact]
        public void Test1() {

        }

        [Fact]
        public async void Test_Default_Http_Get_Success() {
            var factory = ReturnHttpClientFactory();
            var client = new DefaultHttpClient(factory);
            var r = await client.GetAsync<string>("http://www.baidu.com", requestType : RequestType.QueryString);
            Assert.NotEmpty(r);
        }

        private IHttpClientFactory ReturnHttpClientFactory() {
            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(x => x.CreateClient("default")).Returns(new HttpClient());

            return clientFactory.Object;
        }
    }
}
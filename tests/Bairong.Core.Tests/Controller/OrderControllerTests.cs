using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BaiRong.Core;
using SiteServer.API.Controllers.B2C;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;

public class OrderControllerTests
{
    [Fact]
    public void GetOrderStatistic()
    {
        HttpConfiguration config = new HttpConfiguration();
        config.Routes.MapHttpRoute("Default", "api/{controller}/{action}/{id}/{format}");
        HttpServer server = new HttpServer(config);
        using (HttpMessageInvoker client = new HttpMessageInvoker(server))
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Order/GetOrderStatistic"))
            {
                //request.Headers.Add("Cookie", "SITESERVER.ADMINISTRATOR.USERNAME=a2DqxTg7ySo0equals0;SITESERVER.ADMINISTRATOR.AUTH=pp0slash0Nh1B20vPkKhH7eNwr92PoI5o4XfJ4NBLThG0add0YN6zH6zFyDwlj8OygaLuvLWMrHBhyf6tXVX8x4xQ5L8lbLPw61pkkWAG3VK0slash0MxntucHEdh80add04fIKzoXgd6pp7Zm5d0nchB0add0ck0add0WH11PDd4OO6aRdsjMiCAUY65wri4UPwz5Qu0add0m2fmeOVVEz9p5PqrtkSfS1E0add0Zig34NQNPgLIFGnPedK4EPzNYnzOLUeUzfxW5cOAdZMhdrZ5JRAPdGEqFu30slash0Nj0add0815o2p8WunXHISS7Eg0slash0UWmR4E0add0s30add04zoaUeSYJD8dGrBK161am20slash0KWrLntM7gLE90add0IGAjw2YNAcjV3KQ0slash0AScWSvHpi7L;");


                using (HttpResponseMessage response = client.SendAsync(request, CancellationToken.None).Result)
                {
                    HttpContent content = response.Content;

                }
            }
        }

    }

}

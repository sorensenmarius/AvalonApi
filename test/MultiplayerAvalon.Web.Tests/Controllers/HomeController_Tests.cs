using System.Threading.Tasks;
using MultiplayerAvalon.Models.TokenAuth;
using MultiplayerAvalon.Web.Controllers;
using Shouldly;
using Xunit;

namespace MultiplayerAvalon.Web.Tests.Controllers
{
    public class HomeController_Tests: MultiplayerAvalonWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
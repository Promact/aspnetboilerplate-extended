using System.Threading.Tasks;
using BoilerPlateDemo_App.Models.TokenAuth;
using BoilerPlateDemo_App.Web.Controllers;
using Shouldly;
using Xunit;

namespace BoilerPlateDemo_App.Web.Tests.Controllers
{
    public class HomeController_Tests: BoilerPlateDemo_AppWebTestBase
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
using System.Threading.Tasks;
using AspnetBoilerplateExtended.Models.TokenAuth;
using AspnetBoilerplateExtended.Web.Controllers;
using Shouldly;
using Xunit;

namespace AspnetBoilerplateExtended.Web.Tests.Controllers
{
    public class HomeController_Tests: AspnetBoilerplateExtendedWebTestBase
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
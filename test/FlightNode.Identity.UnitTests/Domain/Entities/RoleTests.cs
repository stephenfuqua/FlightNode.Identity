using Xunit;
using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Services.Providers;

namespace FlightNode.Identity.UnitTests.Domain.Entities
{
    public class RoleTests
    {
        [Fact]
        public void ConfirmGetAndSetForDescription()
        {
            var expected = "asdfasdf";
            var system = new Role();

            system.Description = expected;

            var actual = system.Description;

            Assert.Equal(expected, actual);
        }
    }
}

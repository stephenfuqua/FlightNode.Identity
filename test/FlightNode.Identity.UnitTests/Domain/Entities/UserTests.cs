
using Xunit;
using FlightNode.Identity.Domain.Entities;

namespace FlightNode.Identity.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void ConfirmGetAndSetForActive()
        {
            var expected = true;
            var system = new User();

            system.Active = expected;

            var actual = system.Active;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConfirmGetAndSetPhoneNumber()
        {
            var expected = "(555) 555-5555";
            var system = new User();

            system.MobilePhoneNumber = expected;

            var actual = system.MobilePhoneNumber;

            Assert.Equal(expected, actual);
        }

        // TODO: remember how to verify that the "PhoneNumber" data type has been applied to that field

        // TODO: test GenerateUserIdentityAsync, if it is even kept.
    }
}

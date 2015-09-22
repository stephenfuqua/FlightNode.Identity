using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Domain.Logic;
using FlightNode.Identity.Services.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlightNode.Identity.UnitTests.Domain.Logic
{
    public class UserLogicTests
    {
        public class Fixture : IDisposable
        {
            protected Mock<IUserManager> mockUserManager = new Mock<IUserManager>(MockBehavior.Strict);

            protected UserLogic BuildSystem()
            {
                return new UserLogic(mockUserManager.Object);
            }

            public void Dispose()
            {
                mockUserManager.VerifyAll();
            }

        }

        public class ConstructorBehavior : Fixture
        {

            [Fact]
            public void ConfirmWithValidArgument()
            {
                Assert.NotNull(BuildSystem());
            }

            [Fact]
            public void ConfirmThatNullArgumentIsNotAllowed()
            {
                Assert.Throws<ArgumentNullException>(() => new UserLogic(null));
            }
        }

        public class DeactivateAUser : Fixture
        {
            const int id = 23423;

            [Fact]
            public void ConfirmCallToSoftDelete()
            {
                mockUserManager.Setup(x => x.SoftDelete(It.Is<int>(y => y == id)));

                BuildSystem().Deactivate(id);
            }
        }

        public class FindAllUsers : Fixture
        {

            [Fact]
            public void ConfirmEmptyListWhenThereAreNoUsersInTheSystem()
            {
                mockUserManager.SetupGet(x => x.Users)
                    .Returns(new List<User>().AsQueryable());

                var actual = BuildSystem().FindAll();

                Assert.Equal(0, actual.Count());
            }

            public class NoActiveUsers : Fixture
            {
                const int userId = 12312;
                const string email = "asd@asdfasd.com";
                const string phoneNumber = "(555) 555-5555";
                const string userName = "asdfasd";
                const string mobileNumber = "(555) 555-5554";
                const bool active = false;

                private IEnumerable<UserModel> RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName
                    };

                    mockUserManager.SetupGet(x => x.Users)
                        .Returns(new List<User>() {
                           user
                        }.AsQueryable());

                    return BuildSystem().FindAll();
                }

                [Fact]
                public void ConfirmResultCount()
                {
                    var results = RunTheTest();
                    Assert.Equal(0, results.Count());
                }

            }

            public class SingleActiveUsers : Fixture
            {
                const int userId = 12312;
                const string email = "asd@asdfasd.com";
                const string phoneNumber = "(555) 555-5555";
                const string userName = "asdfasd";
                const string mobileNumber = "(555) 555-5554";
                const bool active = true;

                private IEnumerable<UserModel> RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName
                    };

                    mockUserManager.SetupGet(x => x.Users)
                        .Returns(new List<User>() {
                           user
                        }.AsQueryable());

                    return BuildSystem().FindAll();
                }

                [Fact]
                public void ConfirmResultCount()
                {
                    var results = RunTheTest();
                    Assert.Equal(1, results.Count());
                }

                [Fact]
                public void ConfirmMobileNumberIsMapped()
                {
                    Assert.Equal(mobileNumber, RunTheTest().First().MobilePhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMapped()
                {
                    Assert.Equal(phoneNumber, RunTheTest().First().PhoneNumber);
                }

                [Fact]
                public void ConfirmEmailIsMapped()
                {
                    Assert.Equal(email, RunTheTest().First().Email);
                }

                [Fact]
                public void ConfirmUserNameIsMapped()
                {
                    Assert.Equal(userName, RunTheTest().First().UserName);
                }

                [Fact]
                public void ConfirmUserIdIsMapped()
                {
                    Assert.Equal(userId, RunTheTest().First().UserId);
                }
            }

            public class TwoActiveUsers : Fixture
            {
                const int userId = 12312;
                const string email = "asd@asdfasd.com";
                const string phoneNumber = "(555) 555-5555";
                const string userName = "asdfasd";
                const string mobileNumber = "(555) 555-5554";
                const bool active = true;

                private IEnumerable<UserModel> RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName
                    };

                    mockUserManager.SetupGet(x => x.Users)
                        .Returns(new List<User>() {
                            new User { Active = true },
                           user
                        }.AsQueryable());

                    return BuildSystem().FindAll();
                }

                [Fact]
                public void ConfirmResultCount()
                {
                    var results = RunTheTest();
                    Assert.Equal(2, results.Count());
                }

                [Fact]
                public void ConfirmMobileNumberIsMappedForSecondUser()
                {
                    Assert.Equal(mobileNumber, RunTheTest().Skip(1).First().MobilePhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMappedForSecondUser()
                {
                    Assert.Equal(phoneNumber, RunTheTest().Skip(1).First().PhoneNumber);
                }

                [Fact]
                public void ConfirmEmailIsMappedForSecondUser()
                {
                    Assert.Equal(email, RunTheTest().Skip(1).First().Email);
                }

                [Fact]
                public void ConfirmUserNameIsMappedForSecondUser()
                {
                    Assert.Equal(userName, RunTheTest().Skip(1).First().UserName);
                }

                [Fact]
                public void ConfirmUserIdIsMappedForSecondUser()
                {
                    Assert.Equal(userId, RunTheTest().Skip(1).First().UserId);
                }
            }
        }

        public class FindAParticularUserById : Fixture
        {

            [Fact]
            public void ConfirmWhenUserDoesNotExist()
            {
                var id = 1232;
                
                // TODO: confirm that EF is returning a non-null result

                mockUserManager.Setup(x => x.FindByIdAsync(It.Is<int>(y => y == id)))
                    .ReturnsAsync(new User());

                var actual = BuildSystem().FindById(id);

                Assert.NotEqual(id, actual.UserId);
            }

            public class UserDoesExist : Fixture
            {
                const int userId = 12312;
                const string email = "asd@asdfasd.com";
                const string phoneNumber = "(555) 555-5555";
                const string userName = "asdfasd";
                const string mobileNumber = "(555) 555-5554";
                const bool active = true;

                private UserModel RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName
                    };

                    mockUserManager.Setup(x => x.FindByIdAsync(It.Is<int>(y => y == userId)))
                                   .ReturnsAsync(user);

                    return BuildSystem().FindById(userId);
                }
                
                [Fact]
                public void ConfirmPasswordIsEmpty()
                {
                    Assert.Equal(string.Empty, RunTheTest().Password);
                }

                [Fact]
                public void ConfirmMobileNumberIsMapped()
                {
                    Assert.Equal(mobileNumber, RunTheTest().MobilePhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMapped()
                {
                    Assert.Equal(phoneNumber, RunTheTest().PhoneNumber);
                }

                [Fact]
                public void ConfirmEmailIsMapped()
                {
                    Assert.Equal(email, RunTheTest().Email);
                }

                [Fact]
                public void ConfirmUserNameIsMapped()
                {
                    Assert.Equal(userName, RunTheTest().UserName);
                }

                [Fact]
                public void ConfirmUserIdIsMapped()
                {
                    Assert.Equal(userId, RunTheTest().UserId);
                }
            }

        }
    }
}

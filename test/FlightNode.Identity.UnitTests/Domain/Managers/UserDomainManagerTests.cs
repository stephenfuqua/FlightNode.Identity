using FlightNode.Common.Exceptions;
using FlightNode.Identity.Domain.Entities;
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Domain.Logic;
using FlightNode.Identity.Infrastructure.Persistence;
using FlightNode.Identity.Services.Models;
using Microsoft.AspNet.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlightNode.Identity.UnitTests.Domain.Logic
{
    // Only use this class and its test when you need to hash a password and manually update the database
    //public class ManualHashing
    //{
    //    UserManager<User> manager = new UserManager<User>(new UserStore(IdentityDbContext.Create()));
    //}


    public class UserDomainManagerTests
    {
        public class Fixture : IDisposable
        {
            protected Mock<Identity.Domain.Interfaces.IUserPersistence> mockUserManager = new Mock<Identity.Domain.Interfaces.IUserPersistence>(MockBehavior.Strict);

            protected UserDomainManager BuildSystem()
            {
                return new UserDomainManager(mockUserManager.Object);
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
                Assert.Throws<ArgumentNullException>(() => new UserDomainManager(null));
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

        public class CreateUser : Fixture
        {
            [Fact]
            public void NullObjectNotAllowed()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    BuildSystem().Create(null);
                });
            }

            const string givenName = "José";
            const string familyName = "Jalapeño";
            const string primaryPhoneNumber = "(512) 555-2391";
            const string secondaryPhoneNumber = "(651) 234-2349";
            const string userName = "josej";
            const string password = "bien gracias, y tú?";
            const string email = "jose@jalapenos.com";
            const int userId = 2342;

            private UserModel RunTest()
            {
                var input = new UserModel
                {
                    Email = email,
                    FamilyName = familyName,
                    GivenName = givenName,
                    Password = password,
                    PrimaryPhoneNumber = primaryPhoneNumber,
                    SecondaryPhoneNumber = secondaryPhoneNumber,
                    UserName = userName
                };

                return BuildSystem().Create(input);
            }

            [Fact]
            public void ConfirmUserIdIsSet()
            {
                mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.Is<string>(y => y == password)))
                    .Callback((User actual, string p) =>
                    {
                        Assert.Equal(givenName, actual.GivenName);
                        Assert.Equal(familyName, actual.FamilyName);
                        Assert.Equal(primaryPhoneNumber, actual.PhoneNumber);
                        Assert.Equal(secondaryPhoneNumber, actual.MobilePhoneNumber);
                        Assert.Equal(userName, actual.UserName);
                        Assert.Equal(email, actual.Email);
                        Assert.True(actual.Active);
                    })
                    .Returns((User actual, string p) =>
                    {
                        actual.Id = userId;

                        return Task.Run(() => SuccessResult.Create());
                    });



                Assert.Equal(userId, RunTest().UserId);
            }

            [Fact]
            public void ConfirmErrorHandling()
            {
                mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.Is<string>(y => y == password)))
                    .Callback((User actual, string p) =>
                    {
                        Assert.Equal(givenName, actual.GivenName);
                        Assert.Equal(familyName, actual.FamilyName);
                        Assert.Equal(primaryPhoneNumber, actual.PhoneNumber);
                        Assert.Equal(secondaryPhoneNumber, actual.MobilePhoneNumber);
                        Assert.Equal(userName, actual.UserName);
                        Assert.Equal(email, actual.Email);
                        Assert.True(actual.Active);
                    })
                    .Returns((User actual, string p) =>
                    {
                        actual.Id = userId;

                        return Task.Run(() => new IdentityResult(new[] { "something bad happened" }));
                    });


                Assert.Throws<UserException>(() => RunTest().UserId);
            }
        }


        public class UpdateUser : Fixture
        {
            [Fact]
            public void NullObjectNotAllowed()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    BuildSystem().Update(null);
                });
            }

            const string givenName = "José";
            const string familyName = "Jalapeño";
            const string primaryPhoneNumber = "(512) 555-2391";
            const string secondaryPhoneNumber = "(651) 234-2349";
            const string userName = "josej";
            const string password = "bien gracias, y tú?";
            const string email = "jose@jalapenos.com";
            const int userId = 2342;

            private void RunTest()
            {
                var input = new UserModel
                {
                    Email = email,
                    FamilyName = familyName,
                    GivenName = givenName,
                    Password = password,
                    PrimaryPhoneNumber = primaryPhoneNumber,
                    SecondaryPhoneNumber = secondaryPhoneNumber,
                    UserName = userName,
                    UserId = userId
                };

                BuildSystem().Update(input);
            }

            [Fact]
            public void ConfirmUserIdIsSet()
            {
                mockUserManager.Setup(x => x.FindByIdAsync(It.Is<int>(y => y == userId)))
                    .Returns(Task.Run(() => new User()));

                mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                    .Callback((User actual) =>
                    {
                        Assert.Equal(givenName, actual.GivenName);
                        Assert.Equal(familyName, actual.FamilyName);
                        Assert.Equal(primaryPhoneNumber, actual.PhoneNumber);
                        Assert.Equal(secondaryPhoneNumber, actual.MobilePhoneNumber);
                        Assert.Equal(userName, actual.UserName);
                        Assert.Equal(email, actual.Email);
                        Assert.True(actual.Active);
                    })
                    .Returns((User actual) =>
                    {
                        actual.Id = userId;

                        return Task.Run(() => SuccessResult.Create());
                    });

                RunTest();
            }

            [Fact]
            public void ConfirmErrorHandling()
            {
                mockUserManager.Setup(x => x.FindByIdAsync(It.Is<int>(y => y == userId)))
                       .Returns(Task.Run(() => new User()));


                mockUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                    .Returns((User actual) =>
                    {
                        actual.Id = userId;

                        return Task.Run(() => new IdentityResult(new[] { "something bad happened" }));
                    });

                Assert.Throws<UserException>(() => RunTest());
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

            public class SingleActiveUser : Fixture
            {
                const int userId = 12312;
                const string email = "asd@asdfasd.com";
                const string phoneNumber = "(555) 555-5555";
                const string userName = "asdfasd";
                const string mobileNumber = "(555) 555-5554";
                const bool active = true;
                const string familyName = "last";
                const string givenName = "first";

                private IEnumerable<UserModel> RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName,
                        FamilyName = familyName,
                        GivenName = givenName
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
                    Assert.Equal(mobileNumber, RunTheTest().First().SecondaryPhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMapped()
                {
                    Assert.Equal(phoneNumber, RunTheTest().First().PrimaryPhoneNumber);
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

                [Fact]
                public void ConfirmFamilyNameIsMapped()
                {
                    Assert.Equal(familyName, RunTheTest().First().FamilyName);
                }

                [Fact]
                public void ConfirmGivenNameIsMapped()
                {
                    Assert.Equal(givenName, RunTheTest().First().GivenName);
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
                const string familyName = "last";
                const string givenName = "first";

                private IEnumerable<UserModel> RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName,
                        FamilyName = familyName,
                        GivenName = givenName
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
                    Assert.Equal(mobileNumber, RunTheTest().Skip(1).First().SecondaryPhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMappedForSecondUser()
                {
                    Assert.Equal(phoneNumber, RunTheTest().Skip(1).First().PrimaryPhoneNumber);
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

                [Fact]
                public void ConfirmGivenNameIsMappedForSecondUser()
                {
                    Assert.Equal(givenName, RunTheTest().Skip(1).First().GivenName);
                }

                [Fact]
                public void ConfirmFamilyNameIsMappedForSecondUser()
                {
                    Assert.Equal(familyName, RunTheTest().Skip(1).First().FamilyName);
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
                const string familyName = "last";
                const string givenName = "first";

                private UserModel RunTheTest()
                {
                    var user = new User
                    {
                        Active = active,
                        MobilePhoneNumber = mobileNumber,
                        Id = userId,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        UserName = userName,
                        FamilyName = familyName,
                        GivenName = givenName
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
                    Assert.Equal(mobileNumber, RunTheTest().SecondaryPhoneNumber);
                }

                [Fact]
                public void ConfirmPhoneNumberIsMapped()
                {
                    Assert.Equal(phoneNumber, RunTheTest().PrimaryPhoneNumber);
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

                [Fact]
                public void ConfirmFamilylNameIsMapped()
                {
                    Assert.Equal(familyName, RunTheTest().FamilyName);
                }

                [Fact]
                public void ConfirmGivenNameIsMapped()
                {
                    Assert.Equal(givenName, RunTheTest().GivenName);
                }
            }

        }
    }

    public class SuccessResult : IdentityResult
    {
        public SuccessResult() : base(true) { }

        internal static IdentityResult Create()
        {
            return new SuccessResult();
        }
    }
}

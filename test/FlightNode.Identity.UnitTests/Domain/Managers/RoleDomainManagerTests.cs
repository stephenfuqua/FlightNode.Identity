using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNet.Identity;
using FlightNode.Identity.Domain.Interfaces;
using FlightNode.Identity.Domain.Managers;
using FlightNode.Identity.Domain.Entities;
using FlightNode.Common.Exceptions;

namespace FlightNode.Identity.UnitTests.Domain.Managers
{
    public class RoleDomainManagerTests
    {
        public class Fixture : IDisposable
        {
            protected Mock<IRolePersistence> MockRoleStore = new Mock<IRolePersistence>(MockBehavior.Strict);

            public RoleDomainManager BuildSystem()
            {
                return new RoleDomainManager(MockRoleStore.Object);
            }

            public void Dispose()
            {
                MockRoleStore.VerifyAll();
            }
        }

        public class ConstructorTests : Fixture
        {
            [Fact]
            public void ConfirmHappyPath()
            {
                BuildSystem();
            }

            [Fact]
            public void ConfirmThatANullArgumentIsNotAccepted()
            {
                Assert.Throws<ArgumentNullException>(() => { new RoleDomainManager(null); });
            }
        }

        public class FindAll : Fixture
        {
            [Fact]
            public void ConfirmBehaviorWhenThereAreNoRecords()
            {
                MockRoleStore.SetupGet(x => x.Roles)
                        .Returns((new List<Role>()).AsQueryable());

                
                Assert.Equal(0, BuildSystem().FindAll().Count());
            }

            [Fact]
            public void ConfirmThatExceptionsAreRethrownAsServerExceptions()
            {
                MockRoleStore.SetupGet(x => x.Roles)
                    .Throws<InvalidOperationException>();

                Assert.Throws<ServerException>(() => BuildSystem().FindAll());
            }

            public class HappyPath : Fixture
            {
                private const string DESCRIPTION_1 = "System administrators";
                private const int ID_1 = 1;
                private const string NAME_1 = "Admin";
                private const string DESCRIPTION_2 = "Regular users";
                private const int ID_2 = 2;
                private const string NAME_2 = "Users";

                private IEnumerable<Services.Models.RoleModel> RunSystemUnderTest()
                {
                    var list = new List<Role>()
                    {
                        new Role { Description = DESCRIPTION_1, Id = ID_1, Name = NAME_1},
                        new Role { Description = DESCRIPTION_2, Id = ID_2, Name = NAME_2 }
                    };

                    MockRoleStore.SetupGet(x => x.Roles)
                        .Returns(list.AsQueryable());

                    var results = BuildSystem().FindAll();
                    return results;
                }

                [Fact]
                public void ConfirmMultipleRecordCount()
                {
                    Assert.Equal(2, RunSystemUnderTest().Count());
                }

                [Fact]
                public void ConfirmDescriptionInFirstRecord()
                {
                    Assert.Equal(DESCRIPTION_1, RunSystemUnderTest().First().Description);
                }

                [Fact]
                public void ConfirmIdInFirstRecord()
                {
                    Assert.Equal(ID_1, RunSystemUnderTest().First().Id);
                }

                [Fact]
                public void ConfirmNameInFirstRecord()
                {
                    Assert.Equal(NAME_1, RunSystemUnderTest().First().Name);
                }

                [Fact]
                public void ConfirmDescriptionInSecondRecord()
                {
                    Assert.Equal(DESCRIPTION_2, RunSystemUnderTest().Skip(1).First().Description);
                }

                [Fact]
                public void ConfirmIdInSecondRecord()
                {
                    Assert.Equal(ID_2, RunSystemUnderTest().Skip(1).First().Id);
                }

                [Fact]
                public void ConfirmNameInSecondRecord()
                {
                    Assert.Equal(NAME_2, RunSystemUnderTest().Skip(1).First().Name);
                }
            }

        }
    }
}

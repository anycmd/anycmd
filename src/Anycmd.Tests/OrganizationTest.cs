
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.OrganizationViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Moq;
    using Repositories;
    using System;
    using System.Linq;
    using Xunit;

    public class OrganizationTest
    {
        #region OrganizationSet
        [Fact]
        public void OrganizationSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.OrganizationSet.Count());
            Assert.Equal(OrganizationState.VirtualRoot, host.OrganizationSet.First());

            var entityId = Guid.NewGuid();

            OrganizationState organizationById;
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Assert.Equal(2, host.OrganizationSet.Count());
            Assert.True(host.OrganizationSet.TryGetOrganization(entityId, out organizationById));

            host.Handle(new UpdateOrganizationCommand(new OrganizationUpdateInput
            {
                Id = entityId,
                Code = "110",
                Name = "test2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Assert.Equal(2, host.OrganizationSet.Count());
            Assert.True(host.OrganizationSet.TryGetOrganization(entityId, out organizationById));
            Assert.Equal("test2", organizationById.Name);

            host.Handle(new RemoveOrganizationCommand(entityId));
            Assert.False(host.OrganizationSet.TryGetOrganization(entityId, out organizationById));
            Assert.Equal(1, host.OrganizationSet.Count());
        }
        #endregion

        [Fact]
        public void OrganizationCodeMustBeUnique()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.OrganizationSet.Count());

            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            OrganizationState organizationById;
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Assert.Equal(2, host.OrganizationSet.Count());
            Assert.True(host.OrganizationSet.TryGetOrganization(entityId, out organizationById));
            bool catched = false;
            try
            {
                host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
                {
                    Id = entityId2,
                    ParentCode = null,
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }));
                host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
                {
                    Id = entityId2,
                    ParentCode = "100",
                    Code = "100",
                    Name = "测试2",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                }));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(2, host.OrganizationSet.Count());
            }
        }

        [Fact]
        public void OrganizationCanNotRemoveWhenItHasChildOrganizations()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.OrganizationSet.Count());

            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            OrganizationState organizationById;
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = entityId,
                Code = "100",
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = entityId2,
                ParentCode = "100",
                Code = "100100",
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                Icon = null,
            }));
            Assert.Equal(3, host.OrganizationSet.Count());
            Assert.True(host.OrganizationSet.TryGetOrganization(entityId, out organizationById));
            bool catched = false;
            try
            {
                host.Handle(new RemoveOrganizationCommand(entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(3, host.OrganizationSet.Count());
            }
        }

        #region OrganizationSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void OrganizationSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.OrganizationSet.Count());
            Assert.Equal(OrganizationState.VirtualRoot, host.OrganizationSet.First());

            host.RemoveService(typeof(IRepository<Organization>));
            var moOrganizationRepository = host.GetMoqRepository<Organization, IRepository<Organization>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moOrganizationRepository.Setup(a => a.Add(It.Is<Organization>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moOrganizationRepository.Setup(a => a.Update(It.Is<Organization>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moOrganizationRepository.Setup(a => a.Remove(It.Is<Organization>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moOrganizationRepository.Setup<Organization>(a => a.GetByKey(entityId1)).Returns(new Organization { Id = entityId1, Name = name });
            moOrganizationRepository.Setup<Organization>(a => a.GetByKey(entityId2)).Returns(new Organization { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Organization>), moOrganizationRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
                {
                    Id = entityId1,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                    Name = name
                }));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(1, host.OrganizationSet.Count());
            }

            host.Handle(new AddOrganizationCommand(new OrganizationCreateInput
            {
                Id = entityId2,
                Code = "100",
                Description = "test",
                SortCode = 10,
                Icon = null,
                Name = name
            }));
            Assert.Equal(2, host.OrganizationSet.Count());

            catched = false;
            try
            {
                host.Handle(new UpdateOrganizationCommand(new OrganizationUpdateInput
                {
                    Id = entityId2,
                    Code = "100",
                    Description = "test",
                    SortCode = 10,
                    Icon = null,
                    Name = "test2"
                }));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(2, host.OrganizationSet.Count());
                OrganizationState organization;
                Assert.True(host.OrganizationSet.TryGetOrganization(entityId2, out organization));
                Assert.Equal(name, organization.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveOrganizationCommand(entityId2));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                OrganizationState organization;
                Assert.True(host.OrganizationSet.TryGetOrganization(entityId2, out organization));
                Assert.Equal(2, host.OrganizationSet.Count());
            }
        }
        #endregion
    }
}


namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PrivilegeTest
    {
        #region AccountSubjectTypePrivilege
        [Fact]
        public void AccountSubjectTypePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.PrivilegeSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetUserSession(), new GroupCreateInput
            {
                Id = groupId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                TypeCode = "Ac"
            }));
            Guid accountId = Guid.NewGuid();
            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                Password = "111111"
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            var entityId = Guid.NewGuid();

            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            Assert.Equal(0, host.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            var privilegeBigram = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.Equal(accountId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(groupId, privilegeBigram.ObjectInstanceId);

            host.Handle(new UpdatePrivilegeCommand(host.GetUserSession(), new PrivilegeUpdateIo
            {
                Id = entityId,
                AcContent = "this is a test"
            }));
            Assert.Equal(0, host.PrivilegeSet.Count());// 主体为账户的权限记录不驻留在内存中所以为0
            var firstOrDefault = host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            if (
                firstOrDefault != null)
                Assert.Equal("this is a test", firstOrDefault.AcContent);

            host.Handle(new RemovePrivilegeCommand(host.GetUserSession(), entityId));
            Assert.Null(host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId));
        }
        #endregion

        [Fact]
        public void RoleSubjectTypePrivilege()
        {
            var host = TestHelper.GetAcDomain();
            var roleId = Guid.NewGuid();
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            RoleState roleById;
            host.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(host.GetUserSession()));
            Assert.Equal(1, host.RoleSet.Count());
            Assert.True(host.RoleSet.TryGetRole(roleId, out roleById));

            var functionId = Guid.NewGuid();

            FunctionState functionById;
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                SortCode = 10
            }.ToCommand(host.GetUserSession()));
            ResourceTypeState resource;
            Assert.True(host.ResourceTypeSet.TryGetResource(host.ResourceTypeSet.First().Id, out resource));
            Assert.Equal(1, host.FunctionSet.Count());
            Assert.True(host.FunctionSet.TryGetFunction(functionId, out functionById));
            var entityId = Guid.NewGuid();

            host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是角色
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = functionId,
                ObjectType = AcElementType.Function.ToString()
            }));
            PrivilegeState privilegeBigram = host.PrivilegeSet.First(a => a.Id == entityId);
            Assert.NotNull(privilegeBigram);
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId));
            Assert.Equal(roleId, privilegeBigram.SubjectInstanceId);
            Assert.Equal(functionId, privilegeBigram.ObjectInstanceId);

            host.Handle(new UpdatePrivilegeCommand(host.GetUserSession(), new PrivilegeUpdateIo
            {
                Id = entityId,
                AcContent = "this is a test"
            }));
            Assert.Equal("this is a test", host.PrivilegeSet.Single(a => a.Id == entityId).AcContent);

            host.Handle(new RemovePrivilegeCommand(host.GetUserSession(), entityId));
            Assert.Null(host.PrivilegeSet.FirstOrDefault(a => a.Id == entityId));
        }

        [Fact]
        public void CatalogSubjectTypePrivilege()
        {
            // TODO:实现单元测试
        }

        [Fact]
        public void SubjectTypeTest()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.PrivilegeSet.Count());
            UserSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            bool catched = false;
            try
            {
                host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
                {
                    Id = Guid.NewGuid(),
                    SubjectInstanceId = Guid.NewGuid(),
                    SubjectType = "Group",// 用户类别的主体类型只有Account、Catalog、Role。Group不是合法的主体类型所以会报错。
                    AcContent = null,
                    AcContentType = null,
                    ObjectInstanceId = Guid.NewGuid(),
                    ObjectType = AcElementType.Group.ToString()
                }));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.PrivilegeSet.Count());
            }
            catched = false;
            try
            {
                host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
                {
                    Id = Guid.NewGuid(),
                    SubjectInstanceId = Guid.NewGuid(),
                    SubjectType = "InvalidSubjectType",// 非法的Ac元素类型
                    AcContent = null,
                    AcContentType = null,
                    ObjectInstanceId = Guid.NewGuid(),
                    ObjectType = AcElementType.Group.ToString()
                }));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.PrivilegeSet.Count());
            }
            catched = false;
            try
            {
                host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
                {
                    Id = Guid.NewGuid(),
                    SubjectInstanceId = Guid.NewGuid(),// 标识为它的账户不存在，应报错
                    SubjectType = "Account",
                    AcContent = null,
                    AcContentType = null,
                    ObjectInstanceId = Guid.NewGuid(),
                    ObjectType = AcElementType.Group.ToString()
                }));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.PrivilegeSet.Count());
            }

            Guid groupId = Guid.NewGuid();
            host.Handle(new AddGroupCommand(host.GetUserSession(), new GroupCreateInput
            {
                Id = groupId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                ShortName = "",
                SortCode = 10,
                TypeCode = "Ac"
            }));
            Guid accountId = Guid.NewGuid();
            host.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                Password = "111111"
            });
            host.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            catched = false;
            try
            {
                host.Handle(new AddPrivilegeCommand(host.GetUserSession(), new PrivilegeCreateIo
                {
                    Id = Guid.NewGuid(),
                    SubjectInstanceId = accountId,
                    SubjectType = "Account",
                    AcContent = null,
                    AcContentType = null,
                    ObjectInstanceId = groupId,
                    ObjectType = "InvalidObjectType"// 非法的Ac客体类型应报错
                }));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.PrivilegeSet.Count());
            }
        }
    }
}


namespace Anycmd.Tests
{
    using Ac.ViewModels.GroupViewModels;
    using Ac.ViewModels.FunctionViewModels;
    using Ac.ViewModels.PrivilegeViewModels;
    using Ac.ViewModels.RoleViewModels;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Groups;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class PrivilegeTest
    {
        #region AccountSubjectTypePrivilege
        [TestMethod]
        public void AccountSubjectTypePrivilege()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            Guid groupId = Guid.NewGuid();
            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
            acDomain.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                Password = "111111"
            });
            acDomain.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            var entityId = Guid.NewGuid();

            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = accountId,
                SubjectType = UserAcSubjectType.Account.ToString(),// 主体是账户
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = groupId,
                ObjectType = AcElementType.Group.ToString()
            }));
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count()); // 主体为账户的权限记录不驻留在内存中所以为0
            var privilegeBigram = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.AreEqual(accountId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(groupId, privilegeBigram.ObjectInstanceId);

            acDomain.Handle(new UpdatePrivilegeCommand(acDomain.GetAcSession(), new PrivilegeUpdateIo
            {
                Id = entityId,
                AcContent = "this is a test"
            }));
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count());// 主体为账户的权限记录不驻留在内存中所以为0
            var firstOrDefault = acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId);
            if (
                firstOrDefault != null)
                Assert.AreEqual("this is a test", firstOrDefault.AcContent);

            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.IsNull(acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId));
        }
        #endregion

        [TestMethod]
        public void RoleSubjectTypePrivilege()
        {
            var acDomain = TestHelper.GetAcDomain();
            var roleId = Guid.NewGuid();
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            RoleState roleById;
            acDomain.Handle(new RoleCreateInput
            {
                Id = roleId,
                Name = "测试1",
                CategoryCode = "test",
                Description = "test",
                IsEnabled = 1,
                SortCode = 10,
                Icon = null
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.RoleSet.Count());
            Assert.IsTrue(acDomain.RoleSet.TryGetRole(roleId, out roleById));

            var functionId = Guid.NewGuid();

            FunctionState functionById;
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                Code = "fun1",
                Description = string.Empty,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                IsEnabled = 1,
                IsManaged = true,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                SortCode = 10
            }.ToCommand(acDomain.GetAcSession()));
            CatalogState resource;
            Assert.IsTrue(acDomain.CatalogSet.TryGetCatalog(TestHelper.TestCatalogNodeId, out resource));
            Assert.AreEqual(1, acDomain.FunctionSet.Count());
            Assert.IsTrue(acDomain.FunctionSet.TryGetFunction(functionId, out functionById));
            var entityId = Guid.NewGuid();

            acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
            {
                Id = entityId,
                SubjectInstanceId = roleId,
                SubjectType = UserAcSubjectType.Role.ToString(),// 主体是角色
                AcContent = null,
                AcContentType = null,
                ObjectInstanceId = functionId,
                ObjectType = AcElementType.Function.ToString()
            }));
            PrivilegeState privilegeBigram = acDomain.PrivilegeSet.First(a => a.Id == entityId);
            Assert.IsNotNull(privilegeBigram);
            Assert.IsNotNull(acDomain.RetrieveRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.Id == entityId));
            Assert.AreEqual(roleId, privilegeBigram.SubjectInstanceId);
            Assert.AreEqual(functionId, privilegeBigram.ObjectInstanceId);

            acDomain.Handle(new UpdatePrivilegeCommand(acDomain.GetAcSession(), new PrivilegeUpdateIo
            {
                Id = entityId,
                AcContent = "this is a test"
            }));
            Assert.AreEqual("this is a test", acDomain.PrivilegeSet.Single(a => a.Id == entityId).AcContent);

            acDomain.Handle(new RemovePrivilegeCommand(acDomain.GetAcSession(), entityId));
            Assert.IsNull(acDomain.PrivilegeSet.FirstOrDefault(a => a.Id == entityId));
        }

        [TestMethod]
        public void CatalogSubjectTypePrivilege()
        {
            // TODO:实现单元测试
        }

        [TestMethod]
        public void SubjectTypeTest()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            bool catched = false;
            try
            {
                acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
                Assert.IsTrue(catched);
                Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            }
            catched = false;
            try
            {
                acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
                Assert.IsTrue(catched);
                Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            }
            catched = false;
            try
            {
                acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
                Assert.IsTrue(catched);
                Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            }

            Guid groupId = Guid.NewGuid();
            acDomain.Handle(new AddGroupCommand(acDomain.GetAcSession(), new GroupCreateInput
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
            acDomain.RetrieveRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                Code = "test",
                Name = "test",
                Password = "111111"
            });
            acDomain.RetrieveRequiredService<IRepository<Account>>().Context.Commit();
            catched = false;
            try
            {
                acDomain.Handle(new AddPrivilegeCommand(acDomain.GetAcSession(), new PrivilegeCreateIo
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
                Assert.IsTrue(catched);
                Assert.AreEqual(0, acDomain.PrivilegeSet.Count());
            }
        }
    }
}

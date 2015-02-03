
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.ButtonViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.UIViewViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ButtonSetTest
    {
        #region ButtonSet
        [TestMethod]
        public void ButtonSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            ButtonState buttonById;
            ButtonState buttonByCode;
            host.Handle(new ButtonCreateInput
            {
                Id = entityId,
                Code = "btn1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.ButtonSet.Count());
            Assert.IsTrue(host.ButtonSet.ContainsButton(entityId));
            Assert.IsTrue(host.ButtonSet.ContainsButton("btn1"));
            Assert.IsTrue(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsTrue(host.ButtonSet.TryGetButton("btn1", out buttonByCode));
            Assert.AreEqual(buttonByCode, buttonById);
            Assert.IsTrue(ReferenceEquals(buttonById, buttonByCode));

            host.Handle(new ButtonUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Code = "btn2"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.ButtonSet.Count());
            Assert.IsTrue(host.ButtonSet.ContainsButton(entityId));
            Assert.IsTrue(host.ButtonSet.ContainsButton("btn2"));
            Assert.IsTrue(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsTrue(host.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.AreEqual(buttonByCode, buttonById);
            Assert.IsTrue(ReferenceEquals(buttonById, buttonByCode));
            Assert.AreEqual("test2", buttonById.Name);
            Assert.AreEqual("btn2", buttonById.Code);

            host.Handle(new RemoveButtonCommand(host.GetAcSession(), entityId));
            Assert.IsFalse(host.ButtonSet.ContainsButton(entityId));
            Assert.IsFalse(host.ButtonSet.ContainsButton("btn2"));
            Assert.IsFalse(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsFalse(host.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.AreEqual(0, host.ButtonSet.Count());
        }
        #endregion

        #region CanNotDeleteButtonWhenItHasPageButtons
        [TestMethod]
        public void CanNotDeleteButtonWhenItHasPageButtons()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();
            var appSystemId = Guid.NewGuid();
            var functionId = Guid.NewGuid();
            var pageId = functionId;
            var pageButtonId = Guid.NewGuid();

            host.Handle(new ButtonCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.ButtonSet.Count());
            host.Handle(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "app1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(host.GetAcSession()));
            host.Handle(new FunctionCreateInput
            {
                Id = functionId,
                ResourceTypeId = host.ResourceTypeSet.First().Id,
                DeveloperId = host.SysUserSet.GetDevAccounts().First().Id,
                Description = string.Empty,
                Code = "function1",
                IsEnabled = 1,
                IsManaged = true,
                SortCode = 0
            }.ToCommand(host.GetAcSession()));
            host.Handle(new UiViewCreateInput
            {
                Id = functionId
            }.ToCommand(host.GetAcSession()));
            host.Handle(new UiViewButtonCreateInput
            {
                Id = pageButtonId,
                ButtonId = entityId,
                UiViewId = pageId,
                FunctionId = null,
                IsEnabled = 1
            }.ToCommand(host.GetAcSession()));

            bool catched = false;
            try
            {
                host.Handle(new RemoveButtonCommand(host.GetAcSession(), entityId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                ButtonState button;
                Assert.IsTrue(host.ButtonSet.TryGetButton(entityId, out button));
            }

            {
                host.Handle(new RemoveUiViewButtonCommand(host.GetAcSession(), pageButtonId));
                host.Handle(new RemoveButtonCommand(host.GetAcSession(), entityId));
                ButtonState button;
                Assert.IsFalse(host.ButtonSet.TryGetButton(entityId, out button));
            }
        }
        #endregion

        #region ButtonSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void ButtonSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.AreEqual(0, host.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var moButtonRepository = host.GetMoqRepository<Button, IRepository<Button>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string code = "btn1";
            const string name = "测试1";
            host.RemoveService(typeof(IRepository<Button>));
            moButtonRepository.Setup(a => a.Add(It.Is<Button>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moButtonRepository.Setup(a => a.Update(It.Is<Button>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moButtonRepository.Setup(a => a.Remove(It.Is<Button>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moButtonRepository.Setup<Button>(a => a.GetByKey(entityId1)).Returns(new Button { Id = entityId1, Code = code, Name = name });
            moButtonRepository.Setup<Button>(a => a.GetByKey(entityId2)).Returns(new Button { Id = entityId2, Code = code, Name = name });
            host.AddService(typeof(IRepository<Button>), moButtonRepository.Object);


            bool catched = false;
            try
            {
                host.Handle(new ButtonCreateInput
                {
                    Id = entityId1,
                    Code = code,
                    Name = name
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(0, host.ButtonSet.Count());
            }

            host.Handle(new ButtonCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name
            }.ToCommand(host.GetAcSession()));
            Assert.AreEqual(1, host.ButtonSet.Count());

            catched = false;
            try
            {
                host.Handle(new ButtonUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "btn2"
                }.ToCommand(host.GetAcSession()));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                Assert.AreEqual(1, host.ButtonSet.Count());
                ButtonState button;
                Assert.IsTrue(host.ButtonSet.TryGetButton(entityId2, out button));
                Assert.AreEqual(code, button.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveButtonCommand(host.GetAcSession(), entityId2));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(DbException));
                catched = true;
                Assert.AreEqual(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.IsTrue(catched);
                ButtonState button;
                Assert.IsTrue(host.ButtonSet.TryGetButton(entityId2, out button));
                Assert.AreEqual(1, host.ButtonSet.Count());
            }
        }
        #endregion
    }
}

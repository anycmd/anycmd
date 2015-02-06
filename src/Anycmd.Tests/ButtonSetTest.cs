
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
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var entityId = Guid.NewGuid();

            ButtonState buttonById;
            ButtonState buttonByCode;
            acDomain.Handle(new ButtonCreateInput
            {
                Id = entityId,
                Code = "btn1",
                Name = "测试1"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.ButtonSet.Count());
            Assert.IsTrue(acDomain.ButtonSet.ContainsButton(entityId));
            Assert.IsTrue(acDomain.ButtonSet.ContainsButton("btn1"));
            Assert.IsTrue(acDomain.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsTrue(acDomain.ButtonSet.TryGetButton("btn1", out buttonByCode));
            Assert.AreEqual(buttonByCode, buttonById);
            Assert.IsTrue(ReferenceEquals(buttonById, buttonByCode));

            acDomain.Handle(new ButtonUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Code = "btn2"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.ButtonSet.Count());
            Assert.IsTrue(acDomain.ButtonSet.ContainsButton(entityId));
            Assert.IsTrue(acDomain.ButtonSet.ContainsButton("btn2"));
            Assert.IsTrue(acDomain.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsTrue(acDomain.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.AreEqual(buttonByCode, buttonById);
            Assert.IsTrue(ReferenceEquals(buttonById, buttonByCode));
            Assert.AreEqual("test2", buttonById.Name);
            Assert.AreEqual("btn2", buttonById.Code);

            acDomain.Handle(new RemoveButtonCommand(acDomain.GetAcSession(), entityId));
            Assert.IsFalse(acDomain.ButtonSet.ContainsButton(entityId));
            Assert.IsFalse(acDomain.ButtonSet.ContainsButton("btn2"));
            Assert.IsFalse(acDomain.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.IsFalse(acDomain.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.AreEqual(0, acDomain.ButtonSet.Count());
        }
        #endregion

        #region CanNotDeleteButtonWhenItHasPageButtons
        [TestMethod]
        public void CanNotDeleteButtonWhenItHasPageButtons()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
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

            acDomain.Handle(new ButtonCreateInput
            {
                Id = entityId,
                Code = "app1",
                Name = "测试1"
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.ButtonSet.Count());
            acDomain.Handle(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "app1",
                PrincipalId = acDomain.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new FunctionCreateInput
            {
                Id = functionId,
                ResourceTypeId = TestHelper.TestCatalogNodeId,
                DeveloperId = acDomain.SysUserSet.GetDevAccounts().First().Id,
                Description = string.Empty,
                Code = "function1",
                IsEnabled = 1,
                IsManaged = true,
                SortCode = 0
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new UiViewCreateInput
            {
                Id = functionId
            }.ToCommand(acDomain.GetAcSession()));
            acDomain.Handle(new UiViewButtonCreateInput
            {
                Id = pageButtonId,
                ButtonId = entityId,
                UiViewId = pageId,
                FunctionId = null,
                IsEnabled = 1
            }.ToCommand(acDomain.GetAcSession()));

            bool catched = false;
            try
            {
                acDomain.Handle(new RemoveButtonCommand(acDomain.GetAcSession(), entityId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.IsTrue(catched);
                ButtonState button;
                Assert.IsTrue(acDomain.ButtonSet.TryGetButton(entityId, out button));
            }

            {
                acDomain.Handle(new RemoveUiViewButtonCommand(acDomain.GetAcSession(), pageButtonId));
                acDomain.Handle(new RemoveButtonCommand(acDomain.GetAcSession(), entityId));
                ButtonState button;
                Assert.IsFalse(acDomain.ButtonSet.TryGetButton(entityId, out button));
            }
        }
        #endregion

        #region ButtonSetShouldRollbackedWhenPersistFailed
        [TestMethod]
        public void ButtonSetShouldRollbackedWhenPersistFailed()
        {
            var acDomain = TestHelper.GetAcDomain();
            Assert.AreEqual(0, acDomain.ButtonSet.Count());
            AcSessionState.AcMethod.SignIn(acDomain, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var moButtonRepository = acDomain.GetMoqRepository<Button, IRepository<Button>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string code = "btn1";
            const string name = "测试1";
            acDomain.RemoveService(typeof(IRepository<Button>));
            moButtonRepository.Setup(a => a.Add(It.Is<Button>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moButtonRepository.Setup(a => a.Update(It.Is<Button>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moButtonRepository.Setup(a => a.Remove(It.Is<Button>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moButtonRepository.Setup<Button>(a => a.GetByKey(entityId1)).Returns(new Button { Id = entityId1, Code = code, Name = name });
            moButtonRepository.Setup<Button>(a => a.GetByKey(entityId2)).Returns(new Button { Id = entityId2, Code = code, Name = name });
            acDomain.AddService(typeof(IRepository<Button>), moButtonRepository.Object);


            bool catched = false;
            try
            {
                acDomain.Handle(new ButtonCreateInput
                {
                    Id = entityId1,
                    Code = code,
                    Name = name
                }.ToCommand(acDomain.GetAcSession()));
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
                Assert.AreEqual(0, acDomain.ButtonSet.Count());
            }

            acDomain.Handle(new ButtonCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name
            }.ToCommand(acDomain.GetAcSession()));
            Assert.AreEqual(1, acDomain.ButtonSet.Count());

            catched = false;
            try
            {
                acDomain.Handle(new ButtonUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "btn2"
                }.ToCommand(acDomain.GetAcSession()));
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
                Assert.AreEqual(1, acDomain.ButtonSet.Count());
                ButtonState button;
                Assert.IsTrue(acDomain.ButtonSet.TryGetButton(entityId2, out button));
                Assert.AreEqual(code, button.Code);
            }

            catched = false;
            try
            {
                acDomain.Handle(new RemoveButtonCommand(acDomain.GetAcSession(), entityId2));
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
                Assert.IsTrue(acDomain.ButtonSet.TryGetButton(entityId2, out button));
                Assert.AreEqual(1, acDomain.ButtonSet.Count());
            }
        }
        #endregion
    }
}

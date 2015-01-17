
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.AppSystemViewModels;
    using Ac.ViewModels.Infra.ButtonViewModels;
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.UIViewViewModels;
    using Engine.Ac;
    using Engine.Host.Ac.Infra;
    using Engine.Ac.Messages.Infra;
    using Exceptions;
    using Moq;
    using Repositories;
    using System;
    using System.Linq;
    using Xunit;

    public class ButtonSetTest
    {
        #region ButtonSet
        [Fact]
        public void ButtonSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.ButtonSet.Count());

            var entityId = Guid.NewGuid();

            ButtonState buttonById;
            ButtonState buttonByCode;
            host.Handle(new ButtonCreateInput
            {
                Id = entityId,
                Code = "btn1",
                Name = "测试1"
            }.ToCommand());
            Assert.Equal(1, host.ButtonSet.Count());
            Assert.True(host.ButtonSet.ContainsButton(entityId));
            Assert.True(host.ButtonSet.ContainsButton("btn1"));
            Assert.True(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.True(host.ButtonSet.TryGetButton("btn1", out buttonByCode));
            Assert.Equal(buttonByCode, buttonById);
            Assert.True(ReferenceEquals(buttonById, buttonByCode));

            host.Handle(new ButtonUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Code = "btn2"
            }.ToCommand());
            Assert.Equal(1, host.ButtonSet.Count());
            Assert.True(host.ButtonSet.ContainsButton(entityId));
            Assert.True(host.ButtonSet.ContainsButton("btn2"));
            Assert.True(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.True(host.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.Equal(buttonByCode, buttonById);
            Assert.True(ReferenceEquals(buttonById, buttonByCode));
            Assert.Equal("test2", buttonById.Name);
            Assert.Equal("btn2", buttonById.Code);

            host.Handle(new RemoveButtonCommand(entityId));
            Assert.False(host.ButtonSet.ContainsButton(entityId));
            Assert.False(host.ButtonSet.ContainsButton("btn2"));
            Assert.False(host.ButtonSet.TryGetButton(entityId, out buttonById));
            Assert.False(host.ButtonSet.TryGetButton("btn2", out buttonByCode));
            Assert.Equal(0, host.ButtonSet.Count());
        }
        #endregion

        #region CanNotDeleteButtonWhenItHasPageButtons
        [Fact]
        public void CanNotDeleteButtonWhenItHasPageButtons()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.ButtonSet.Count());

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
            }.ToCommand());
            Assert.Equal(1, host.ButtonSet.Count());
            host.Handle(new AppSystemCreateInput
            {
                Id = appSystemId,
                Code = "app1",
                Name = "app1",
                PrincipalId = host.SysUserSet.GetDevAccounts().First().Id
            }.ToCommand());
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
            }.ToCommand());
            host.Handle(new UiViewCreateInput
            {
                Id = functionId
            }.ToCommand());
            host.Handle(new UiViewButtonCreateInput
            {
                Id = pageButtonId,
                ButtonId = entityId,
                UiViewId = pageId,
                FunctionId = null,
                IsEnabled = 1
            }.ToCommand());

            bool catched = false;
            try
            {
                host.Handle(new RemoveButtonCommand(entityId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                ButtonState button;
                Assert.True(host.ButtonSet.TryGetButton(entityId, out button));
            }

            {
                host.Handle(new RemoveUiViewButtonCommand(pageButtonId));
                host.Handle(new RemoveButtonCommand(entityId));
                ButtonState button;
                Assert.False(host.ButtonSet.TryGetButton(entityId, out button));
            }
        }
        #endregion

        #region ButtonSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void ButtonSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.ButtonSet.Count());

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
                }.ToCommand());
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
                Assert.Equal(0, host.ButtonSet.Count());
            }

            host.Handle(new ButtonCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name
            }.ToCommand());
            Assert.Equal(1, host.ButtonSet.Count());

            catched = false;
            try
            {
                host.Handle(new ButtonUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "btn2"
                }.ToCommand());
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
                Assert.Equal(1, host.ButtonSet.Count());
                ButtonState button;
                Assert.True(host.ButtonSet.TryGetButton(entityId2, out button));
                Assert.Equal(code, button.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveButtonCommand(entityId2));
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
                ButtonState button;
                Assert.True(host.ButtonSet.TryGetButton(entityId2, out button));
                Assert.Equal(1, host.ButtonSet.Count());
            }
        }
        #endregion
    }
}

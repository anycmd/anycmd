
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.DicViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class DicSetTest
    {
        #region DicSet
        [Fact]
        public void DicSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.DicSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dicId = Guid.NewGuid();
            var dicItemId = Guid.NewGuid();

            DicState dicById;
            DicState dicByCode;
            host.Handle(new DicCreateInput
            {
                Id = dicId,
                Code = "dic1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(2, host.DicSet.Count());
            Assert.True(host.DicSet.TryGetDic(dicId, out dicById));
            Assert.True(host.DicSet.TryGetDic("dic1", out dicByCode));
            Assert.Equal(dicByCode, dicById);
            Assert.True(ReferenceEquals(dicById, dicByCode));

            host.Handle(new DicUpdateInput
            {
                Id = dicId,
                Name = "test2",
                Code = "dic2"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(2, host.DicSet.Count());
            Assert.True(host.DicSet.TryGetDic(dicId, out dicById));
            Assert.True(host.DicSet.TryGetDic("dic2", out dicByCode));
            Assert.Equal(dicByCode, dicById);
            Assert.True(ReferenceEquals(dicById, dicByCode));
            Assert.Equal("test2", dicById.Name);
            Assert.Equal("dic2", dicById.Code);

            host.Handle(new RemoveDicCommand(host.GetAcSession(), dicId));
            Assert.False(host.DicSet.TryGetDic(dicId, out dicById));
            Assert.False(host.DicSet.TryGetDic("dic2", out dicByCode));
            Assert.Equal(1, host.DicSet.Count());

            // 开始测试DicItem
            host.Handle(new DicCreateInput
            {
                Id = dicId,
                Code = "dic1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(2, host.DicSet.Count());
            Assert.True(host.DicSet.TryGetDic(dicId, out dicById));
            DicItemState dicItemById;
            DicItemState dicItemByCode;
            host.Handle(new DicItemCreateInput
            {
                Id = dicItemId,
                IsEnabled = 1,
                DicId = dicId,
                SortCode = 0,
                Description = string.Empty,
                Code = "dicItem1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(1, host.DicSet.GetDicItems(dicById).Count());
            Assert.True(host.DicSet.TryGetDicItem(dicItemId, out dicItemById));
            Assert.True(host.DicSet.TryGetDicItem(dicById, "dicItem1", out dicItemByCode));
            Assert.Equal(dicItemByCode, dicItemById);
            Assert.True(ReferenceEquals(dicItemById, dicItemByCode));

            host.Handle(new DicItemUpdateInput
            {
                Id = dicItemId,
                Name = "test2",
                Code = "dicItem2"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(1, host.DicSet.GetDicItems(dicById).Count);
            Assert.True(host.DicSet.TryGetDicItem(dicItemId, out dicItemById));
            Assert.True(host.DicSet.TryGetDicItem(dicById, "dicItem2", out dicItemByCode));
            Assert.Equal(dicItemByCode, dicItemById);
            Assert.True(ReferenceEquals(dicItemById, dicItemByCode));
            Assert.Equal("test2", dicItemById.Name);
            Assert.Equal("dicItem2", dicItemById.Code);

            host.Handle(new RemoveDicItemCommand(host.GetAcSession(), dicItemId));
            Assert.False(host.DicSet.TryGetDicItem(dicItemId, out dicItemById));
            Assert.False(host.DicSet.TryGetDicItem(dicById, "dicItem2", out dicItemByCode));
            Assert.Equal(0, host.DicSet.GetDicItems(dicById).Count);
        }
        #endregion

        #region CanNotDeleteDicWhenItHasDicItems
        [Fact]
        public void CanNotDeleteDicWhenItHasDicItems()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.DicSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var dicId = Guid.NewGuid();

            host.Handle(new DicCreateInput
            {
                Id = dicId,
                Code = "dic1",
                Name = "测试1"
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(2, host.DicSet.Count());

            host.Handle(new DicItemCreateInput
            {
                Id = Guid.NewGuid(),
                DicId = dicId,
                Name = "item1",
                Code = "item1",
                IsEnabled = 1,
                SortCode = 10,
                Description = string.Empty,
            }.ToCommand(host.GetAcSession()));

            bool catched = false;
            try
            {
                host.Handle(new RemoveDicCommand(host.GetAcSession(), dicId));
            }
            catch (ValidationException)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                DicState dic;
                Assert.True(host.DicSet.TryGetDic(dicId, out dic));
            }
        }
        #endregion

        #region DicSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void DicSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(1, host.DicSet.Count());
            AcSessionState.SignIn(host, new Dictionary<string, object>
            {
                {"loginName", "test"},
                {"password", "111111"},
                {"rememberMe", "rememberMe"}
            });
            var moDicRepository = host.GetMoqRepository<Dic, IRepository<Dic>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string code = "dic1";
            const string name = "测试1";
            host.RemoveService(typeof(IRepository<Dic>));
            moDicRepository.Setup(a => a.Add(It.Is<Dic>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moDicRepository.Setup(a => a.Update(It.Is<Dic>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moDicRepository.Setup(a => a.Remove(It.Is<Dic>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moDicRepository.Setup<Dic>(a => a.GetByKey(entityId1)).Returns(new Dic { Id = entityId1, Code = code, Name = name });
            moDicRepository.Setup<Dic>(a => a.GetByKey(entityId2)).Returns(new Dic { Id = entityId2, Code = code, Name = name });
            host.AddService(typeof(IRepository<Dic>), moDicRepository.Object);


            bool catched = false;
            try
            {
                host.Handle(new DicCreateInput
                {
                    Id = entityId1,
                    Code = code,
                    Name = name
                }.ToCommand(host.GetAcSession()));
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
                Assert.Equal(1, host.DicSet.Count());
            }

            host.Handle(new DicCreateInput
            {
                Id = entityId2,
                Code = code,
                Name = name
            }.ToCommand(host.GetAcSession()));
            Assert.Equal(2, host.DicSet.Count());

            catched = false;
            try
            {
                host.Handle(new DicUpdateInput
                {
                    Id = entityId2,
                    Name = "test2",
                    Code = "dic2"
                }.ToCommand(host.GetAcSession()));
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
                Assert.Equal(2, host.DicSet.Count());
                DicState dic;
                Assert.True(host.DicSet.TryGetDic(entityId2, out dic));
                Assert.Equal(code, dic.Code);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveDicCommand(host.GetAcSession(), entityId2));
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
                DicState dic;
                Assert.True(host.DicSet.TryGetDic(entityId2, out dic));
                Assert.Equal(2, host.DicSet.Count());
            }
        }
        #endregion
    }
}


namespace Anycmd.Tests
{
    using Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using System;

    /// <summary>
    /// 实体标识采用“及早生成策略”，实体标识生成后是不能更改的。
    /// </summary>
    [TestClass]
    public class EntityIdTest
    {
        [TestMethod]
        [ExpectedException(typeof(AnycmdException))]
        public void EntityID_Can_Not_Change()
        {
            var entity = new TestEntity { Id = Guid.NewGuid() };
            entity.Id = Guid.NewGuid();
        }

        [TestMethod]
        public void EntityID_Can_Init()
        {
            var entity = new TestEntity();
            Assert.IsTrue(entity.Id == Guid.Empty);
            var id = Guid.NewGuid();
            entity.Id = id;
            Assert.IsTrue(entity.Id == id);
        }
    }

    public class TestEntity : EntityObject
    {

    }
}

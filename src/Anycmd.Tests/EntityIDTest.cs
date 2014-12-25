
namespace Anycmd.Tests
{
    using Exceptions;
    using Model;
    using System;
    using Xunit;

    /// <summary>
    /// 实体标识采用“及早生成策略”，实体标识生成后是不能更改的。
    /// </summary>
    public class EntityIdTest
    {
        [Fact]
        public void EntityID_Can_Not_Change()
        {
            Assert.Throws<CoreException>(() =>
            {
                var entity = new TestEntity {Id = Guid.NewGuid()};
                entity.Id = Guid.NewGuid();
            });
        }

        [Fact]
        public void EntityID_Can_Init()
        {
            var entity = new TestEntity();
            Assert.True(entity.Id == Guid.Empty);
            var id = Guid.NewGuid();
            entity.Id = id;
            Assert.True(entity.Id == id);
        }
    }

    public class TestEntity : EntityObject
    {

    }
}

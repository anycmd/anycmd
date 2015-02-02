﻿
namespace Anycmd.Engine
{
    using Commands;
    using System;

    /// <summary>
    /// 表示表达更新给定的实体的命令的.NET类型。
    /// <remarks>.NET类型命名规则：Update'EntityType'Command。如UpdateUserCommand、UpdateRoleCommand、UpdateCatalogCommand。</remarks>
    /// </summary>
    public abstract class UpdateEntityCommand<TEntityUpdateInput> : Command where TEntityUpdateInput : class, IEntityUpdateInput
    {
        protected UpdateEntityCommand(IAcSession userSession, TEntityUpdateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = userSession;
            this.Input = input;
        }

        public IAcSession AcSession { get; private set; }

        public TEntityUpdateInput Input { get; private set; }
    }
}

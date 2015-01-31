
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class OntologyBase : EntityBase, IOntology
    {
        private bool _isSystem = true;

        #region Ctor
        protected OntologyBase()
        {
        }
        #endregion

        #region Public Properties
        public string Triggers { get; set; }
        /// <summary>
        /// Web服务有效状态
        /// </summary>
        public bool ServiceIsAlive { get; set; }
        /// <summary>
        /// 命令提供程序主键
        /// </summary>
        public Guid MessageProviderId { get; set; }
        /// <summary>
        /// 数据提供程序主键
        /// </summary>
        public Guid EntityProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSystem
        {
            get { return _isSystem; }
            set
            {
                if (_isSystem == false && value == true)
                {
                    throw new AnycmdException("不能更改系统属性");
                }
                _isSystem = value;
            }
        }
        /// <summary>
        /// 是否是目录型
        /// </summary>
        public bool IsCataloguedEntity { get; set; }
        /// <summary>
        /// 是否是逻辑删除型
        /// </summary>
        public bool IsLogicalDeletionEntity { get; set; }
        /// <summary>
        /// 本体库
        /// </summary>
        public Guid EntityDatabaseId { get; set; }
        /// <summary>
        /// 命令库
        /// </summary>
        public Guid MessageDatabaseId { get; set; }
        /// <summary>
        /// 架构
        /// </summary>
        public string EntitySchemaName { get; set; }
        /// <summary>
        /// 架构
        /// </summary>
        public string MessageSchemaName { get; set; }
        /// <summary>
        /// 表
        /// </summary>
        public string EntityTableName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanAction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanCommand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CanEvent { get; set; }
        /// <summary>
        /// 成功接收的命令缓冲区尺寸。单位为1条命令
        /// </summary>
        public int ReceivedMessageBufferSize { get; set; }
        /// <summary>
        /// 命令执行器休眠时间段长度（毫秒）
        /// </summary>
        public int ExecutorSleepTimeSpan { get; set; }
        /// <summary>
        /// 待执行命令提供程序批幅
        /// </summary>
        public int ExecutorLoadCount { get; set; }
        /// <summary>
        /// 待分发命令提供程序批幅
        /// </summary>
        public int DispatcherLoadCount { get; set; }
        /// <summary>
        /// 命令分发器休眠时间段长度（毫秒）
        /// </summary>
        public int DispatcherSleepTimeSpan { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 编辑框宽度
        /// </summary>
        public int EditWidth { get; set; }
        /// <summary>
        /// 编辑框高度
        /// </summary>
        public int EditHeight { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }
        #endregion
    }
}

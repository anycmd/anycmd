
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示本体数据访问实体
    /// <remarks>
    /// 在计算机科学与信息科学领域，本体是指一种“形式化的，对于共享概念体系的明确而又详细的说明”。
    /// 本体提供的是一种共享词表，也就是特定领域之中那些存在着的对象类型或概念及其属性和相互关系；
    /// 或者说，本体就是一种特殊类型的术语集，具有结构化的特点，且更加适合于在计算机系统之中使用；
    /// 或者说，本体实际上就是对特定领域之中某套概念及其相互之间关系的形式化表达（formal representation）。
    /// 本体是人们以自己兴趣领域的知识为素材，运用信息科学的本体论原理而编写出来的作品（artifacts）。
    /// 本体一般可以用来针对该领域的属性进行推理，亦可用于定义该领域（也就是对该领域进行建模）。
    /// 此外，有时人们也会将“本体”称为“本体论”。
    /// ——以上文字摘自维基百科
    /// 
    /// 本体（Ontology）是一个概念框架，给出一套词汇标识一套概念，这些词汇就是术语。本体本身也需要标识，
    /// 比如我们说“物理学”，物理学这三个字就标识了本体。对于本体这个形而上学的东西读者没必要纠结，
    /// 只需知道在数据交换平台中计算机是使用编码来标识本体的就可以了。如“JS（教师）”标识一个本体，
    /// “XS（学生）”标识一个本体，在教师本体概念框架下“当前状态”指的是教师的状态，而在学生本体概念框架下
    /// “当前状态”指的是学生的状态，再比如：在教师本体框架下有“所教学科”这样的概念但在学生本体下没有，
    /// 而在学生本体下会有“家长联系电话”这样的概念但在教师本体下没有。数据交换平台使用固定的唯一的编码来识别每一个本体，
    /// 将教师编码为JS，学生编码为XS，这种编码是不区分大小写的字符串，JS和XS就是“本体码（OntologyCode）”，
    /// 关于本体码在下文有专门的一章可进一步了解。本体完整的涵盖一类实体的所有属性，而不同的节点对同一类实体
    /// 所关注的属性往往不同，每一个节点所关注的通常是一类实体所有属性的一个子集，这个子集我们称为主题（Subject）或者话题（Topic），
    /// 有时也称作数据上下文（Data Context），如教育一线通系统关心JS的手机号码而不关心教师的出生年月，
    /// 教育一线通的上下文就是“发手机短信”这件事，数据上下文采用分类法进行识别。
    /// 其次，实体上有众多属性，如教师的性别、出生日期、身份证件号、所属组织结构等，这类属性我们称为“元素（Element）”
    /// 或者“属性（Attribute）”，为了表明它从属于一个本体才有意义的性质通常也称作“本体元素”，同样平台使用唯一的字符串编码
    /// 来识别每一个本体元素，这个编码我们称作本体元素码（ElementCode）。既然本体元素是对实体数据项的描述那么为什么不直接称为
    /// “数据项”而要使用“本体元素”这个新词呢？这是因为，本体元素上封装的不仅仅是实体的数据项，还有其它一些类别的信息，如本体元素级权限等。
    /// 本体元素在下文也有专门的一节来描述。而，数据模式是在本体下使用实体、属性、关系、数据类型定义的固定的数据结构。本体元素编码了实体属性，
    /// 定义了数据项的类型、值域、是否可空等一系列数据属性。
    /// </remarks>
    /// </summary>
    public class Ontology : OntologyBase, IAggregateRoot
    {
        public Ontology() { }

        public static Ontology Create(IOntologyCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Ontology
            {
                Id = input.Id.Value,
                CanAction = false,
                CanCommand = false,
                CanEvent = false,
                Code = input.Code,
                Name = input.Name,
                DeletionStateCode = 0,
                Description = input.Description,
                EditHeight = input.EditHeight,
                EditWidth = input.EditWidth,
                DispatcherLoadCount = 100,
                DispatcherSleepTimeSpan = 10000,
                EntityDatabaseId = input.EntityDatabaseId,
                EntityProviderId = input.EntityProviderId,
                EntitySchemaName = input.EntitySchemaName,
                EntityTableName = input.EntityTableName,
                Icon = input.Icon,
                ExecutorLoadCount = 100,
                IsEnabled = input.IsEnabled,
                ExecutorSleepTimeSpan = 10000,
                IsLogicalDeletionEntity = false,
                IsOrganizationalEntity = false,
                IsSystem = false,
                MessageDatabaseId = input.MessageDatabaseId,
                MessageProviderId = input.MessageProviderId,
                MessageSchemaName = input.MessageSchemaName,
                ReceivedMessageBufferSize = 1000,
                ServiceIsAlive = false,
                SortCode = input.SortCode,
                Triggers = string.Empty
            };
        }

        public void Update(IOntologyUpdateIo input)
        {
            this.Code = input.Code;
            this.MessageSchemaName = input.MessageSchemaName;
            this.MessageDatabaseId = input.MessageDatabaseId;
            this.Description = input.Description;
            this.EditHeight = input.EditHeight;
            this.EditWidth = input.EditWidth;
            this.EntityDatabaseId = input.EntityDatabaseId;
            this.EntityProviderId = input.EntityProviderId;
            this.EntitySchemaName = input.EntitySchemaName;
            this.EntityTableName = input.EntityTableName;
            this.Icon = input.Icon;
            this.IsEnabled = input.IsEnabled;
            this.MessageProviderId = input.MessageProviderId;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}

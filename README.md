Anycmd <a target="_blank" href="http://shang.qq.com/wpa/qunwpa?idkey=7c53e6d84b1c9ca2a31a1cd58e33cea5f32ffa9ef8427996a8c0a15d9fe3ef35"><img border="0" src="http://pub.idqqimg.com/wpa/images/group.png" alt=".NET 开源权限引擎" title=".NET 开源权限引擎" />QQ交流群:306029222</a>
<a href="http://www.oschina.net/question/tag/anycmd" target="_blank">Anycmd在oschina的关键字</a>
======
#概括梁山开源权限引擎干了什么？怎么干的？

权限引擎设计为嵌入到应用系统中运行，需要每个平台都有个引擎才行。我们一起定义一套标准，每个权限引擎都有能力以安全中心节点运行。安全管理员在安全中心节点使用权限引擎管理系统面向所有接入的业务系统中的功能和资源授权，安全管理员的操作所产生的权限数据会及时高效的同步到相关的业务节点中去，各个业务节点只与嵌入在自己中的本地的权限引擎打交道不与远端节点打交道。

开发具体业务系统的程序员需要做的是在外部主体在本系统中的分神进入受控区域时告诉权限引擎“我是谁？我要干什么？我的输入是什么？”，并在外部主体在本系统的分神试图把信息带出本系统时告诉权限引擎“我是谁？我干了什么？我要带出什么？”。

“我是谁（我从哪里来，我身上有什么）？我要干什么（我要用哪"种"手段作用哪"种"客体）？我的输入是什么（用于定位哪"个"手段和哪"个"客体，资源是空间，是一种分层结构，对空间的定位没有精细程度上的极限，可以到达任意精细的粒度，比如到达具体字段的具体取值）？” -> “我是谁？我干了什么？我要把什么带出系统？”。

莫非以上就是传说中的三大哲学问题？

**方法论**：
主客体是 **空间** ， **操作** 是 **运动** ，运动是主客体的 **变化** ，运动是空间的变化，空间的变化是 **时间** 。一切的一切都是如何管理时间与空间？如何 **管理** 这个时空？大体是：时空 **集合化** （时间集合化，空间集合化，时空集合化）、时空 **类型化** 、时间与空间 **场化** 、时空与人的构造 **匹配化** 、人与时空 **组织化** 、慢慢 **优化** "人与时空"并慢慢 **本能化** 。

我只会.net，现在只写了.net的，java、golang、nodejs、ruby、rust的虽然有头有主了可是大家可能还没动手写，希望有兴趣的朋友参与进来搭把手。

#权限系统干了什么？

给出一套方法，将系统中的所有功能标识出来，组织起来，托管起来，将所有的数据组织起来标识出来托管起来，
然后提供一个简单的唯一的接口，这个接口的一端是应用系统一端是权限引擎。权限引擎所回答的只是：谁是否对某资源具有实施
某个动作（运动、计算）的权限。返回的结果只有：有、没有、权限引擎异常了。

##文档 http://anycmd.github.io/anycmd/
### [背景][7]
### [介绍][1]
### [访问控制元素][2]
### [访问控制字段][3]
### [EntityType和ResourceType和Ontology三者的区别与联系][4]
### [数据交换协议指南][6]
### [模拟事务——为SQL和NO SQL统一事务工作][5]

[1]: https://github.com/anycmd/anycmd/wiki/overview
[2]: https://github.com/anycmd/anycmd/wiki/elements
[3]: https://github.com/anycmd/anycmd/wiki/acField
[4]: https://github.com/anycmd/anycmd/wiki/ontology-resourceType-entityType
[6]: https://github.com/anycmd/anycmd/wiki/edi-guideline
[5]: https://github.com/anycmd/anycmd/wiki/ACID
[7]: https://github.com/anycmd/anycmd/wiki/Background
* 1，视频介绍《anycmd筑基》 http://www.tudou.com/programs/view/4jXoarZKwCk/

##演示站 http://www.anycmd.com:20150/

#Anycmd简介

Anycmd是一个.net平台的完全开源的，完整支持Rbac的（包括核心Rbac、通用角色层次Rbac、静态职责分离Rbac和动态责任分离Rbac），将会支持xacml的通用的权限框架、中间件、解决方案。完整的Rbac规范所定义的能力只是anycmd所提供的能力集的一个子集。
如果您感兴趣的话现在可以先观察Anycmd的源码，期待您为Anycmd提供帮助确保她走在正确的道路上。

框架、中间件、解决方案是它的三种使用模式：<br/>
**框架模式：**
	引用一两个必要的dll或者相应的源码，它跟您的应用系统运行在一起，您有能力完全控制anycmd，您需要自己提供UI层，但anycmd自带的UI层也是可用的。通过面向anycmd遍布各处的扩展点编程使用者有机会有能力实现自己个性化的需求；<br/>
**中间件模式：**
	引用一两个必要的dll和一些资源文件，它可以跟您的应用系统运行在一起，它提供UI层但您也可以删除并自主提供，它按照最佳实践提供默认配置，您可以通过调整配置比如自定义插件来满足或接近满足您的需求。可以把anycmd中间件看作是一个独立的系统，只不过它可以和你的应用系统运行在同一个进程、同一个或不同的AppDomain。<br/>
**解决方案模式：**
	提供一整套AC最佳实践、方法论，使用者有走向最佳实践的意愿。

##如何使用

anycmd不仅提供了来自Rbac国际规范文档的IRbacService接口，还提供了一整套稳定的、功能完备的、风格一致的、流畅的api。框架使用起来非常简单，限定在AC领域内，基本会做到在权限方面的每一个需求都刚好有一个流畅的风格一致的api。编程的时候只需要通过一套风格一致的流畅的api告诉框架我们希望做什么，然后框架就去做了。但是如果能够明白访问控制系统做事情的逻辑的话会更容易使用那些api。
事实上anycmd很容易地就完整实现了对IRbacService的支持，因为对IRbacService的实现不需要书写专门的逻辑，因为anycmd的api是比Rbac所定义的能力集更大的，只需直接委托给anycmd的api就完整实现了IRbacService。
##运行
找到Web.config的BootDbConnString应用设置项，将这个连接字符串的密码修改成您的密码。Web.config中只有这一个引导库连接字符串，
其余数据库的连接字符串在Anycmd引导库的RDatabase表中，请使用SqlServer管理工具找到Anycmd数据库的RDatabase表修改其密码项。
##测试账户
成功运行后转到“用户”模块，所有现有账户密码都是“111111”六个1。

##路线图
* 1，书写单元测试；
* 2，书写教程；
* 3，替换掉UI层，去除试用版的miniui框架（并非miniui不好，而是权限引擎需要为使用者尽可能去除非开源的或非足够开源的事物）；
* 4，内置数据交换系统，用以各业务系统与中心系统间的权限数据交换；
* 5，支持Javascript；https://github.com/sebastienros/jint/
* 6，支持LDAP（轻量目录访问协议）。
* 7，优化；发布1.0版本；
* 8，支持SAML；
* 9，支持工作流http://slickflow.codeplex.com/
* 10，支持Xacml；http://docs.oasis-open.org/xacml/3.0/

---
## 感谢
* [Apworks](https://github.com/daxnet/Apworks/) @ [Apache License 2.0]
* [Jint](https://github.com/sebastienros/jint/) @ [BSD 2-Clause License]

---
## 学习资源
* javacript引擎：https://github.com/sebastienros/jint/
* actor框架：https://github.com/akkadotnet/akka.net/
* nosql文档数据库：https://github.com/mbdavid/LiteDB/

---
##授权协议
The MIT license。

QQ交流群:306029222 <a target="_blank" href="http://shang.qq.com/wpa/qunwpa?idkey=7c53e6d84b1c9ca2a31a1cd58e33cea5f32ffa9ef8427996a8c0a15d9fe3ef35"><img border="0" src="http://pub.idqqimg.com/wpa/images/group.png" alt=".NET 开源权限引擎" title=".NET 开源权限引擎" /></a>

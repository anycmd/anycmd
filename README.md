<img border="0" src="http://git.oschina.net/anycmd/anycmd/raw/master/docs/logoSmall.png" alt="logo" title="logo"> Anycmd <a target="_blank" href="http://shang.qq.com/wpa/qunwpa?idkey=7c53e6d84b1c9ca2a31a1cd58e33cea5f32ffa9ef8427996a8c0a15d9fe3ef35"><img border="0" src="http://pub.idqqimg.com/wpa/images/group.png" alt=".NET 开源权限引擎" title=".NET 开源权限引擎"></a>
======
#权限系统干了什么？

给出一套方法，将系统中的所有功能标识出来，组织起来，托管起来，将所有的数据组织起来标识出来托管起来，
然后提供一个简单的唯一的接口，这个接口的一端是应用系统一端是权限引擎。权限引擎所回答的只是：谁是否对某资源具有实施
某个动作（运动、计算）的权限。返回的结果只有：有、没有、权限引擎异常了。

#wiki https://github.com/anycmd/anycmd/wiki

* 1，视频介绍《anycmd筑基》 http://www.tudou.com/programs/view/4jXoarZKwCk/

<div>如果您觉得anycmd人还不错，不妨打赏一杯咖啡吧 **[打赏][1]**</div>

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

anycmd不仅提供了来自Rbac国际规范文档的IRbacService接口
http://git.oschina.net/anycmd/anycmd/blob/master/src/Anycmd/IRbacService.cs
还提供了一整套稳定的、功能完备的、风格一致的、流畅的api。框架使用起来非常简单，限定在AC领域内，基本会做到在权限方面的每一个需求都刚好有一个流畅的风格一致的api。编程的时候只需要通过一套风格一致的流畅的api告诉框架我们希望做什么，然后框架就去做了。但是如果能够明白访问控制系统做事情的逻辑的话会更容易使用那些api。
事实上anycmd很容易地就完整实现了对IRbacService的支持，因为对IRbacService的实现不需要书写专门的逻辑，因为anycmd的api是比Rbac所定义的能力集更大的，只需直接委托给anycmd的api就完整实现了IRbacService。
##运行
找到Web.config的BootDbConnString应用设置项，将这个连接字符串的密码修改成您的密码。Web.config中只有这一个引导库连接字符串，
其余数据库的连接字符串在Anycmd引导库的RDatabase表中，请使用SqlServer管理工具找到Anycmd数据库的RDatabase表修改其密码项。
<img src="http://git.oschina.net/anycmd/anycmd/raw/master/docs/AnycmdMisSite.jpg" />
##测试账户
成功运行后转到“用户”模块，所有现有账户密码都是“111111”六个1。

##路线图
* 1，书写单元测试；
* 2，书写教程；
* 3，替换掉UI层，去除试用版的miniui框架；考虑使用extjs
* 4，内置数据交换系统，用以各业务系统与中心系统间的权限数据交换；
* 5，支持Javascript；
* 6，支持REPL。“读取-求值-输出”循环(英语:Read-Eval-Print Loop,简称REPL)是一个简单的,交互式的编程环境。
* 7，优化；发布1.0版本；
* 8，支持SAML；
* 9，基于slickflow（原名wf5）支持工作流http://slickflow.codeplex.com/
* 10，支持Xacml；

---
## 感谢
* [Apworks](https://github.com/daxnet/Apworks/) @ [Apache License 2.0]
* [Jint](https://github.com/sebastienros/jint/) @ [BSD 2-Clause License]
---
## 学习资源
* javacript引擎：https://github.com/sebastienros/jint/
* actor框架：https://github.com/akkadotnet/akka.net/
* nosql文档数据库：https://github.com/ravendb/ravendb/

---
##授权协议
The MIT license。
<img src="http://git.oschina.net/anycmd/anycmd/raw/master/docs/MIT.png" />
><img border="0" src="http://git.oschina.net/anycmd/anycmd/raw/master/docs/logoBig.png" alt="logo" title="logo"> QQ交流群:306029222 <a target="_blank" href="http://shang.qq.com/wpa/qunwpa?idkey=7c53e6d84b1c9ca2a31a1cd58e33cea5f32ffa9ef8427996a8c0a15d9fe3ef35"><img border="0" src="http://pub.idqqimg.com/wpa/images/group.png" alt=".NET 开源权限引擎" title=".NET 开源权限引擎"></a>

[1]: http://meiweihezi.com/dashang/dashang.php?id=ZGs1NjM=
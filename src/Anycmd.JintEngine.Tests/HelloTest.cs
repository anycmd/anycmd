
using Jint.Runtime;

namespace Anycmd.JintEngine.Tests
{
    using Engine.Ac;
    using Engine.Host.Ac.Identity;
    using Jint;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class HelloTest
    {
        [TestMethod]
        public void FuncTest()
        {
            var engine = new Engine()
                .SetValue("hello", (Func<string, string>)(a => a));
            engine.Execute(@"
      function getWord(word) { 
        return hello(word);
      };

      getWord('Hello World');");
            var value = engine.GetCompletionValue().ToObject();
            Assert.AreEqual("Hello World", value);
            engine.Execute("getWord('worldbye')");
            value = engine.GetCompletionValue().ToObject();
            Assert.AreEqual("worldbye", value);
        }

        [TestMethod]
        public void ActionTest()
        {
            var engine = new Engine()
        .SetValue("log", new Action<object>(Console.WriteLine))
        ;

            engine.Execute(@"
              function hello() { 
                log('Hello World');
              };

              hello();
            ");
        }

        [TestMethod]
        public void VarTest()
        {
            var square = new Engine()
                .SetValue("x", 3) // define a new variable
                .Execute("x * x") // execute a statement
                .GetCompletionValue() // get the latest statement completion value
                .ToObject() // converts the value to .NET
                ;
            Assert.AreEqual(9.0, square);
        }

        [TestMethod]
        public void EntityTest()
        {
            var account = new Account
            {
                Name = "xuefly"
            };

            var engine = new Engine()
                .SetValue("p", account)
                .Execute("p.Name = '薛兴帅'")
                ;
            Assert.AreEqual("薛兴帅", account.Name);
        }

        /*
         * 后缀为State的类的写法有一点调整，因为这些对象后续会通过类似Jint这样的.NET javascript引擎发布出来，
         * 使用javascript面向这些对象书写安全策略。为了防止外部的javascript修改.NET对象的状态，所有属性的set访问器方法需去掉。
         */
        [TestMethod]
        public void StateTest()
        {
            var account = AccountState.Create(new Account { Name = "xuefly" });

            var engine = new Engine()
                .SetValue("p", account)
                .Execute("p.Name = '薛兴帅'")
                ;
            Assert.AreEqual("xuefly", account.Name);
        }

        [TestMethod]
        public void InternalModifyTest()
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "xuefly"
            };
            var state = AccountState.Create(account);
            account.Name = "薛兴帅";

            string msg = null;
            try
            {
                var engine = new Engine()
                    .SetValue("state", state)
                    .Execute("state.InternalModify(state)");
            }
            catch (JavaScriptException e)
            {
                msg = e.Message;
            }
            Assert.AreEqual("Object has no method 'InternalModify'", msg);

            Assert.AreEqual("xuefly", state.Name);
        }
    }
}

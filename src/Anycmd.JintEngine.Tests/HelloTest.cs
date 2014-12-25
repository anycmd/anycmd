
namespace Anycmd.JintEngine.Tests
{
    using Engine.Ac;
    using Engine.Host.Ac.Identity;
    using Jint;
    using System;
    using Xunit;

    public class HelloTest
    {
        [Fact]
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
            Assert.Equal("Hello World", value);
            engine.Execute("getWord('worldbye')");
            value = engine.GetCompletionValue().ToObject();
            Assert.Equal("worldbye", value);
        }

        [Fact]
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

        [Fact]
        public void VarTest()
        {
            var square = new Engine()
                .SetValue("x", 3) // define a new variable
                .Execute("x * x") // execute a statement
                .GetCompletionValue() // get the latest statement completion value
                .ToObject() // converts the value to .NET
                ;
            Assert.Equal(9.0, square);
        }

        [Fact]
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
            Assert.Equal("薛兴帅", account.Name);
        }

        /*
         * TODO:后缀为State的类的写法有一点调整，因为这些对象后续会通过类似Jint这样的.NET javascript引擎发布出来，
         * 使用javascript面向这些对象书写安全策略。为了防止外部的javascript修改.NET对象的状态，所有属性的set访问器方法需去掉。
         */
        [Fact]
        public void StateTest()
        {
            var account = AccountState.Create(new Account { Name = "xuefly" });

            var engine = new Engine()
                .SetValue("p", account)
                .Execute("p.Name = '薛兴帅'")
                ;
            Assert.Equal("xuefly", account.Name);
        }
    }
}

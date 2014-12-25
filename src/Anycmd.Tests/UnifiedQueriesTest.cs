
namespace Anycmd.Tests
{
    using Query.UnifiedQueries;
    using Query.UnifiedQueries.Compilers;
    using System;
    using System.Linq;
    using Xunit;

    public class UnifiedQueriesTest
    {
        [Fact]
        public void QueryTest()
        {
            var querySpecification = QuerySpecification.LoadFromFile("QuerySpecificationSample.xml");
            var compiler = new LambdaExpressionCompiler<Customer>();
            var customers = GetAllCustomers();
            foreach (var customer in customers.Where(compiler.Compile(querySpecification).Compile()))
            {
                Console.WriteLine(
                    @"FirstName: {0}, LastName: {1}, YearlyIncome: {2}",
                    customer.FirstName,
                    customer.LastName,
                    customer.YearlyIncome);
            }
        }

        private static Customer[] GetAllCustomers()
        {
            return new[]
                       {
                           new Customer { FirstName = "Sunny", LastName = "Chen", YearlyIncome = 10000 },
                           new Customer { FirstName = "PeterJam", LastName = "Yo", YearlyIncome = 10000 },
                           new Customer { FirstName = "PeterR", LastName = "Ko", YearlyIncome = 50000 },
                           new Customer { FirstName = "FPeter", LastName = "Law", YearlyIncome = 70000 },
                           new Customer { FirstName = "Jim", LastName = "Peter", YearlyIncome = 30000 }
                       };
        }

        public class Customer
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public Decimal YearlyIncome { get; set; }
        }
    }
}

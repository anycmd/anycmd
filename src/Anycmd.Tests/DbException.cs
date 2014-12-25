using System;

namespace Anycmd.Tests
{
    public class DbException : Exception
    {
        public DbException() { }

        public DbException(string message)
            : base(message)
        {

        }
    }
}

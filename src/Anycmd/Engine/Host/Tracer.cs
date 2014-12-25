using System;

namespace Anycmd.Engine.Host
{
    public class Tracer
    {
        public static ITracer Instance = new NullTracer();

        public class NullTracer : ITracer
        {
            public void WriteDebug(string error) { }

            public void WriteDebug(string format, params object[] args) { }

            public void WriteWarning(string warning) { }

            public void WriteWarning(string format, params object[] args) { }

            public void WriteError(Exception ex) { }

            public void WriteError(string error) { }

            public void WriteError(string format, params object[] args) { }
        }
    }
}

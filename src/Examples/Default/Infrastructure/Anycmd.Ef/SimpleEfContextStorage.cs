
namespace Anycmd.Ef
{
    using Model;
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// 
    /// </summary>
    public sealed class SimpleEfContextStorage : DisposableObject, IEfContextStorage
    {
        private readonly ConcurrentDictionary<string, EfRepositoryContext> _repositoryContexts = new ConcurrentDictionary<string, EfRepositoryContext>(StringComparer.OrdinalIgnoreCase);

        public void SetRepositoryContext(EfRepositoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._repositoryContexts[context.EfDbContextName] = context;
        }

        public EfRepositoryContext GetRepositoryContext(string key)
        {
            EfRepositoryContext cxt;
            if (!this._repositoryContexts.TryGetValue(key, out cxt))
            {
                return null;
            }

            return cxt;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in _repositoryContexts.Values)
                {
                    item.Dispose();
                }
            }
        }
    }
}
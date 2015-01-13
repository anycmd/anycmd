
namespace Anycmd.MongoDb
{
    using MongoDB.Driver;
    using Repositories;
    using System;

    public interface IMongoDdRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// Gets a <see cref="IMongoDdRepositoryContextSettings"/> instance which contains the settings
        /// information used by current context.
        /// </summary>
        IMongoDdRepositoryContextSettings Settings { get; }
        /// <summary>
        /// Gets the <see cref="MongoCollection"/> instance by the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> object.</param>
        /// <returns>The <see cref="MongoCollection"/> instance.</returns>
        MongoCollection GetCollectionForType(Type type);
    }
}


namespace Anycmd.Ef
{
    using Model;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;

    public static class ServiceContainerExtension
    {
        public static void RegisterRepository(this IAcDomain host, IEnumerable<string> efDbContextNames, params Assembly[] assemblies)
        {
            var repositoryContexts = efDbContextNames.Select(item => new EfRepositoryContext(host, item)).ToList();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(type))
                    {
                        var repositoryType = typeof(CommonRepository<>);
                        var genericInterface = typeof(IRepository<>);
                        repositoryType = repositoryType.MakeGenericType(type);
                        genericInterface = genericInterface.MakeGenericType(type);
                        foreach (var repositoryContext in repositoryContexts)
                        {
                            if (TryGetType(repositoryContext, type))
                            {
                                var repository = Activator.CreateInstance(repositoryType, host, repositoryContext.EfDbContextName);
                                host.AddService(genericInterface, repository);
                            }
                        }
                    }
                }
            }
            foreach (var item in repositoryContexts)
            {
                item.Dispose();
            }
        }
        public static void RegisterQuery(this IAcDomain host, params Assembly[] assemblies)
        {
            Register(host, "Query", assemblies);
        }

        private static void Register(IAcDomain host, string endsWith, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && type.Name.EndsWith(endsWith))
                    {
                        var defaultInterface = type.GetInterface("I" + type.Name);
                        if (defaultInterface != null)
                        {
                            host.AddService(defaultInterface, Activator.CreateInstance(type, host));
                        }
                    }
                }
            }
        }

        private static bool TryGetType(EfRepositoryContext repositoryContext, Type entityType)
        {
            var metadataWorkspace = ((IObjectContextAdapter)repositoryContext.DbContext).ObjectContext.MetadataWorkspace;
            EdmType edmType;
            return metadataWorkspace.TryGetType(entityType.Name, "AnycmdModel", DataSpace.CSpace, out edmType);
        }
    }
}

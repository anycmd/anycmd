
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
        public static void RegisterRepository(this IAcDomain acDomain, IEnumerable<string> efDbContextNames, params Assembly[] assemblies)
        {
            var repositoryContexts = efDbContextNames.Select(item => new EfRepositoryContext(acDomain, item)).ToList();
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
                                var repository = Activator.CreateInstance(repositoryType, acDomain, repositoryContext.EfDbContextName);
                                acDomain.AddService(genericInterface, repository);
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
        public static void RegisterQuery(this IAcDomain acDomain, params Assembly[] assemblies)
        {
            Register(acDomain, "Query", assemblies);
        }

        private static void Register(IAcDomain acDomain, string endsWith, params Assembly[] assemblies)
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
                            acDomain.AddService(defaultInterface, Activator.CreateInstance(type, acDomain));
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

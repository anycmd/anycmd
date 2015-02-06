
namespace Anycmd.Tests
{
    using Model;
    using Moq;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TestHelper
    {
        public static readonly Guid TestCatalogNodeId = new Guid("7C801EA5-05A8-4F16-A92C-BB89DDCA5A3D");

        public static IAcDomain GetAcDomain()
        {
            var acDomain = new MoqAcDomain().Init();

            return acDomain;
        }

        public static Mock<TRepository> GetMoqRepository<TEntity, TRepository>(this IAcDomain acDomain)
            where TEntity : class, IAggregateRoot
            where TRepository : class, IRepository<TEntity>
        {

            var moRepository = new Mock<TRepository>();
            var context = new MoqRepositoryContext(acDomain);
            moRepository.Setup(a => a.Context).Returns(context);
            moRepository.Setup(a => a.Add(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Remove(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Update(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns((TEntity)null);
            moRepository.Setup(a => a.AsQueryable()).Returns(new List<TEntity>().AsQueryable());

            return moRepository;
        }

        public static void RegisterRepository(this IAcDomain acDomain, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(type))
                    {
                        var repositoryType = typeof(MoqCommonRepository<>);
                        var genericInterface = typeof(IRepository<>);
                        repositoryType = repositoryType.MakeGenericType(type);
                        genericInterface = genericInterface.MakeGenericType(type);
                        var repository = Activator.CreateInstance(repositoryType, acDomain);
                        acDomain.AddService(genericInterface, repository);
                    }
                }
            }
        }
    }
}

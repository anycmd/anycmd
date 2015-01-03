
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
        public static IAcDomain GetAcDomain()
        {
            return new MoqAcDomain().Init();
        }

        public static Mock<TRepository> GetMoqRepository<TEntity, TRepository>(this IAcDomain host)
            where TEntity : class, IAggregateRoot
            where TRepository : class, IRepository<TEntity>
        {

            var moRepository = new Mock<TRepository>();
            var context = new MoqRepositoryContext(host);
            moRepository.Setup(a => a.Context).Returns(context);
            moRepository.Setup(a => a.Add(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Remove(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Update(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns((TEntity)null);
            moRepository.Setup(a => a.AsQueryable()).Returns(new List<TEntity>().AsQueryable());

            return moRepository;
        }

        public static void RegisterRepository(this IAcDomain host, params Assembly[] assemblies)
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
                        var repository = Activator.CreateInstance(repositoryType, host);
                        host.AddService(genericInterface, repository);
                    }
                }
            }
        }
    }
}

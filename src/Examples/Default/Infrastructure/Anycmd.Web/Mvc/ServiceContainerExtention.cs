using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Web.Mvc;

namespace Anycmd.Web.Mvc
{
    public static class ServiceContainerExtention
    {
        public static IServiceContainer RegisterController<T>(this IServiceContainer c) where T : IController
        {
            c.RegisterControllers(typeof(T));
            return c;
        }

        public static IServiceContainer RegisterControllers(this IServiceContainer c, params Type[] controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                if (type.IsController())
                {
                    c.AddService(type, Activator.CreateInstance(type));
                }
            }

            return c;
        }

        public static IServiceContainer RegisterControllers(this IServiceContainer c, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                c.RegisterControllers(assembly.GetExportedTypes());
            }
            return c;
        }
        /// <summary>
        /// Determines whether the specified type is a controller
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if type is a controller, otherwise false</returns>
        public static bool IsController(this Type type)
        {
            return type != null
                   && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                   && !type.IsAbstract
                   && typeof(IController).IsAssignableFrom(type);
        }
    }
}

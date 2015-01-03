
namespace Anycmd.Engine.Host
{
    using Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Threading.Tasks;

    /// <summary>
    /// 一个线程安全的服务对象容器。
    /// </summary>
    public class AnycmdServiceContainer : IServiceContainer, IDisposable
    {
        private readonly ConcurrentStack<IServiceProvider> _fallbackProviders = new ConcurrentStack<IServiceProvider>();
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly List<Type> _servicesToDispose = new List<Type>();
        private readonly Dictionary<Type, object> _taskCompletionSources = new Dictionary<Type, object>(); // object = TaskCompletionSource<T> for various T

        /// <summary>
        /// 初始化一个 <c>AnycmdServiceContainer</c> 类型的对象。
        /// </summary>
        public AnycmdServiceContainer()
        {
            _services.Add(typeof(AnycmdServiceContainer), this);
            _services.Add(typeof(IServiceContainer), this);
        }

        /// <summary>
        /// 添加一个服务提供程序到可靠提供程序栈。
        /// </summary>
        /// <param name="provider">将被添加的服务提供程序。</param>
        public void AddFallbackProvider(IServiceProvider provider)
        {
            this._fallbackProviders.Push(provider);
        }

        /// <summary>
        /// 获取给定类型的服务对象。
        /// </summary>
        /// <param name="serviceType">服务对象的类型。</param>
        /// <returns>服务对象。可能返回null。</returns>
        public object GetService(Type serviceType)
        {
            object instance;
            lock (_services)
            {
                if (_services.TryGetValue(serviceType, out instance))
                {
                    var callback = instance as ServiceCreatorCallback;
                    if (callback != null)
                    {
                        Object log;
                        if (_services.TryGetValue(typeof(ILoggingService), out log))
                        {
                            var loggingService = log as ILoggingService;
                            if (loggingService != null)
                            {
                                loggingService.Debug("Service startup: " + serviceType);
                            }
                        }
                        instance = callback(this, serviceType);
                        if (instance != null)
                        {
                            _services[serviceType] = instance;
                            OnServiceInitialized(serviceType, instance);
                        }
                        else
                        {
                            _services.Remove(serviceType);
                        }
                    }
                }
            }
            if (instance != null)
                return instance;
            foreach (var fallbackProvider in _fallbackProviders)
            {
                instance = fallbackProvider.GetService(serviceType);
                if (instance != null)
                    return instance;
            }
            return null;
        }

        public void Dispose()
        {
            Type[] disposableTypes;
            lock (_services)
            {
                disposableTypes = _servicesToDispose.ToArray();
                //services.Clear();
                _servicesToDispose.Clear();
            }
            // dispose services in reverse order of their creation
            for (int i = disposableTypes.Length - 1; i >= 0; i--)
            {
                IDisposable disposable = null;
                lock (_services)
                {
                    object serviceInstance;
                    if (_services.TryGetValue(disposableTypes[i], out serviceInstance))
                    {
                        disposable = serviceInstance as IDisposable;
                        if (disposable != null)
                            _services.Remove(disposableTypes[i]);
                    }
                }
                if (disposable != null)
                {
                    Object log;
                    if (_services.TryGetValue(typeof(ILoggingService), out log))
                    {
                        var loggingService = log as ILoggingService;
                        if (loggingService != null)
                        {
                            loggingService.Debug("Service shutdown: " + disposableTypes[i]);
                        }
                    }
                    disposable.Dispose();
                }
            }
        }

        void OnServiceInitialized(Type serviceType, object serviceInstance)
        {
            var disposableService = serviceInstance as IDisposable;
            if (disposableService != null)
                _servicesToDispose.Add(serviceType);

            dynamic taskCompletionSource;
            if (_taskCompletionSources.TryGetValue(serviceType, out taskCompletionSource))
            {
                _taskCompletionSources.Remove(serviceType);
                taskCompletionSource.SetResult((dynamic)serviceInstance);
            }
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            lock (_services)
            {
                _services.Add(serviceType, serviceInstance);
                OnServiceInitialized(serviceType, serviceInstance);
            }
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            AddService(serviceType, serviceInstance);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            lock (_services)
            {
                _services.Add(serviceType, callback);
            }
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            AddService(serviceType, callback);
        }

        public void RemoveService(Type serviceType)
        {
            lock (_services)
            {
                object instance;
                if (!_services.TryGetValue(serviceType, out instance)) return;
                _services.Remove(serviceType);
                var disposableInstance = instance as IDisposable;
                if (disposableInstance != null)
                    _servicesToDispose.Remove(serviceType);
            }
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            RemoveService(serviceType);
        }

        public Task<T> GetFutureService<T>()
        {
            Type serviceType = typeof(T);
            lock (_services)
            {
                object instance;
                if (_services.TryGetValue(serviceType, out instance))
                {
                    return Task.FromResult((T)instance);
                }
                else
                {
                    object taskCompletionSource;
                    if (_taskCompletionSources.TryGetValue(serviceType, out taskCompletionSource))
                    {
                        return ((TaskCompletionSource<T>)taskCompletionSource).Task;
                    }
                    else
                    {
                        var tcs = new TaskCompletionSource<T>();
                        _taskCompletionSources.Add(serviceType, tcs);
                        return tcs.Task;
                    }
                }
            }
        }
    }
}

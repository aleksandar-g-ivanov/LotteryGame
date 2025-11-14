using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Lib.DependencyRegister
{
    public class DependencyContainer
    {
        IServiceProvider _provider;

        static DependencyContainer _instance;
        public static T Get<T>() => Instance.Resolve<T>();
        public static DependencyContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }
        public static DependencyContainer Create(Action<IServiceCollection> config)
        {
            return Instance.Initialize(config);
        }

        public DependencyContainer Initialize(Action<IServiceCollection> config)
        {
            var services = new ServiceCollection();
            config(services);
            _provider = services.BuildServiceProvider();
            return this;
        }
        public T Resolve<T>() where T : notnull
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("DependencyContainer is not initialized. Call 'Initialize()' first.");
            }

            return _provider.GetRequiredService<T>();
        }
        public void Release()
        {
            if (_provider != null)
            {
                if (_provider is IDisposable disposableProvider)
                {
                    disposableProvider.Dispose();
                }
                _provider = null;
            }
        }
    }
}

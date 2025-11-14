using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Lib.Factories
{    
    public class FactoryRegister
    {
        readonly IServiceProvider _provider;
        readonly Dictionary<Type, object> _factories = new();

        public FactoryRegister(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static FactoryRegister Create(IServiceProvider provider) => new(provider);

        public FactoryRegister Register<T, TFactory>() where TFactory : IFactory<T>
        {
            object factory = ActivatorUtilities.CreateInstance(_provider, typeof(TFactory));
            _factories[typeof(T)] = factory;
            return this;
        }

        public IFactory<T> GetFactory<T>()
        {
            object factory = _factories[typeof(T)];
            return (IFactory<T>)factory;
        }

        public T Create<T>() => GetFactory<T>().Create();
    }
}

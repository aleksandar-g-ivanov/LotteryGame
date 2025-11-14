namespace Lottery.Lib.Factories
{
    public interface IFactory<T>
    {
        public T Create();
    }
}

namespace CoinKeeper.Logic.IoCContainer
{
    public interface IContainerAdapter
    {
        T Resolve<T>() where T : class;
    }
}
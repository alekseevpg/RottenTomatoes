﻿using TinyIoC;

namespace CoinKeeper.Logic.IoCContainer
{
    public class TinyIocAdapter : IContainerAdapter
    {
        private readonly TinyIoCContainer _tinyIocContainer;

        public TinyIocAdapter(TinyIoCContainer container)
        {
            _tinyIocContainer = container;
        }

        public T Resolve<T>() where T : class
        {
            return _tinyIocContainer.Resolve<T>();
        }
    }
}
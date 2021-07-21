using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Mediation
{
    /// <inheritdoc />
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, List<ISubscriber>> _subscribers;

        /// <summary>
        /// Creates a new instance of <see cref="MessageBus"/>.
        /// </summary>
        public MessageBus()
        {
            _subscribers = new Dictionary<Type, List<ISubscriber>>();
        }

        /// <inheritdoc />
        public ISubscriber Subscribe<T>(Action<T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<ISubscriber>();

            var subscriber = new Subscriber<T>(callback);
            subscriber.Disposed += OnDisposed;
            _subscribers[type].Add(subscriber);

            return subscriber;
        }

        private void OnDisposed(object sender, EventArgs e)
        {
            var subscriber = (ISubscriber) sender;
            subscriber.Disposed -= OnDisposed;

            foreach (var (_, value) in _subscribers)
            {
                if (!value.Contains(sender))
                    continue;

                value.Remove(subscriber);
                break;
            }

            ClearEmpty();
        }

        private void ClearEmpty()
        {
            var keys = _subscribers.Keys.ToList();
            foreach (var key in keys)
            {
                if (!_subscribers[key].Any())
                    _subscribers.Remove(key);
            }
        }

        /// <inheritdoc />
        public void Publish<T>(T item)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                return;

            var subscribers = _subscribers[type].Select(x => (Subscriber<T>) x).ToList();
            foreach (var subscriber in subscribers)
                subscriber.Invoke(item);
        }
    }
}
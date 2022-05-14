// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Windows.Threading;

namespace SniffCore.Mediation
{
    /// <summary>
    ///     Represents a single subscription on an object type.
    /// </summary>
    /// <typeparam name="T">The type of the object this subscriber is listen for.</typeparam>
    public class Subscriber<T> : ISubscriber
    {
        private readonly Guid _instance;
        private Action<T> _callback;
        private Dispatcher _dispatcher;

        internal Subscriber(Action<T> callback)
        {
            _callback = callback;
            _instance = Guid.NewGuid();
        }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public ISubscriber On(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            return this;
        }

        /// <summary>
        ///     Disposes this subscription.
        /// </summary>
        public void Dispose()
        {
            _callback = null;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        internal void Invoke(T obj)
        {
            if (_dispatcher != null)
                _dispatcher.Invoke(() => _callback?.Invoke(obj));
            else
                _callback?.Invoke(obj);
        }

        /// <summary>
        ///     Compares this subscriber to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the object is the same; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return _instance.Equals(((Subscriber<T>) obj)._instance);
        }

        /// <summary>
        ///     Returns the hashcode representing this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _instance.GetHashCode();
        }
    }
}
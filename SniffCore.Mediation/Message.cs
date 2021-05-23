//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Mediation
{
    internal interface IMessage
    {
        public Type Key { get; set; }
        bool IsAlive { get; }
        void Remove(SubscribeToken subscribeToken);
    }

    internal class Message<T> : IMessage
    {
        internal Message()
        {
            Key = typeof(T);
            Actions = new Dictionary<SubscribeToken, WeakAction<T>>();
        }

        private Dictionary<SubscribeToken, WeakAction<T>> Actions { get; }

        public Type Key { get; set; }

        public bool IsAlive
        {
            get
            {
                RemoveDead();
                return Actions.Count > 0;
            }
        }

        public void Remove(SubscribeToken subscribeToken)
        {
            if (Actions.ContainsKey(subscribeToken))
                Actions.Remove(subscribeToken);
        }

        internal void Send(T parameter)
        {
            var actions = Actions.Values.ToList();
            foreach (var action in actions)
                action.GetAction()(parameter);
        }

        internal SubscribeToken Add(Action<T> callback)
        {
            var listenToken = new SubscribeToken();
            Actions.Add(listenToken, new WeakAction<T>(callback));
            return listenToken;
        }

        private void RemoveDead()
        {
            var actions = Actions.Where(pair => !pair.Value.IsAlive).ToList();
            foreach (var pair in actions)
                Actions.Remove(pair.Key);
        }
    }
}
//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global

namespace SniffCore.Mediation
{
    /// <summary>
    ///     Brings possibility to communicate between modules not knowing each other.
    /// </summary>
    public static class MessageBus
    {
        private static readonly List<IMessage> _messages;

        static MessageBus()
        {
            _messages = new List<IMessage>();
        }

        /// <summary>
        ///     Subscribes to an object been sent.
        /// </summary>
        /// <typeparam name="T">The object type to listen for.</typeparam>
        /// <param name="callback">The callback to call if the corresponding object got sent.</param>
        /// <returns>The subscribe token to maintain the subscription.</returns>
        public static SubscribeToken Subscribe<T>(Action<T> callback)
        {
            var messageContainer = (Message<T>) GetOrCreateMessage<T>();
            return messageContainer.Add(callback);
        }

        /// <summary>
        ///     Sends an object to ite subscribers.
        /// </summary>
        /// <typeparam name="T">The object type to send.</typeparam>
        /// <param name="implementation">The object other may listen for.</param>
        public static void Notify<T>(T implementation)
        {
            RemoveDead();
            var messageContainer = (Message<T>) GetMessage<T>();
            messageContainer?.Send(implementation);
        }

        /// <summary>
        ///     Removes a specific subscriptions by the token.
        /// </summary>
        /// <param name="subscribeToken">The token for the subscription to remove.</param>
        public static void Unsubscribe(SubscribeToken subscribeToken)
        {
            foreach (var message in _messages)
                message.Remove(subscribeToken);
            RemoveDead();
        }

        /// <summary>
        ///     Removes all subscriptions by their token.
        /// </summary>
        /// <param name="listenTokens">A collection of tokens to remove.</param>
        public static void Unsubscribe(IEnumerable<SubscribeToken> listenTokens)
        {
            foreach (var listenToken in listenTokens)
                Unsubscribe(listenToken);
        }

        /// <summary>
        ///     Removes all subscriptions by their token.
        /// </summary>
        /// <param name="listenTokens">A collection of tokens to remove.</param>
        public static void Unsubscribe(params SubscribeToken[] listenTokens)
        {
            foreach (var listenToken in listenTokens)
                Unsubscribe(listenToken);
        }

        private static IMessage GetOrCreateMessage<T>()
        {
            var messageContainer = GetMessage<T>();
            if (messageContainer != null)
                return messageContainer;

            messageContainer = new Message<T>();
            _messages.Add(messageContainer);

            return messageContainer;
        }

        private static IMessage GetMessage<T>()
        {
            return _messages.FirstOrDefault(m => m.Key == typeof(T));
        }

        private static void RemoveDead()
        {
            _messages.RemoveAll(m => !m.IsAlive);
        }
    }
}
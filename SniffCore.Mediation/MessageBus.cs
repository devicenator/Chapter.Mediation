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
    /// <example>
    ///     <code lang="csharp">
    /// <![CDATA[
    /// public class MessageData
    /// {
    ///     public MessageData(string message)
    ///     {
    ///         Message = message;
    ///     }
    /// 
    ///     public string Message { get; }
    /// }
    /// 
    /// public class ViewModel1
    /// {
    ///     public void Send(string message)
    ///     {
    ///         MessageBus.Notify(new MessageData(message));
    ///     }
    /// }
    /// 
    /// public class ViewModel2 : IDisposable
    /// {
    ///     private readonly SubscribeToken _subscribeToken;
    /// 
    ///     public ViewModel2()
    ///     {
    ///         _subscribeToken = MessageBus.Subscribe<MessageData>(OnMessageReceived);
    ///     }
    /// 
    ///     private void OnMessageReceived(MessageData obj)
    ///     {
    ///         var message = obj.Message;
    ///     }
    /// 
    ///     public void Dispose()
    ///     {
    ///         MessageBus.Unsubscribe(_subscribeToken);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
        /// <exception cref="ArgumentNullException">callback is null.</exception>
        public static SubscribeToken Subscribe<T>(Action<T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var messageContainer = GetOrCreateMessage<T>();
            return messageContainer.Add(callback);
        }

        /// <summary>
        ///     Sends an object to ite subscribers.
        /// </summary>
        /// <typeparam name="T">The object type to send.</typeparam>
        /// <param name="implementation">The object other may listen for.</param>
        /// <exception cref="ArgumentNullException">implementation is null.</exception>
        public static void Notify<T>(T implementation)
        {
            if (implementation == null)
                throw new ArgumentNullException(nameof(implementation));

            RemoveDead();
            var messageContainer = GetMessage<T>();
            messageContainer?.Send(implementation);
        }

        /// <summary>
        ///     Removes a specific subscriptions by the token.
        /// </summary>
        /// <param name="subscribeToken">The token for the subscription to remove.</param>
        /// <exception cref="ArgumentNullException">subscribeToken is null.</exception>
        public static void Unsubscribe(SubscribeToken subscribeToken)
        {
            if (subscribeToken == null)
                throw new ArgumentNullException(nameof(subscribeToken));

            foreach (var message in _messages)
                message.Remove(subscribeToken);
            RemoveDead();
        }

        /// <summary>
        ///     Removes all subscriptions by their token.
        /// </summary>
        /// <param name="listenTokens">A collection of tokens to remove.</param>
        /// <exception cref="ArgumentNullException">listenTokens is null.</exception>
        public static void Unsubscribe(IEnumerable<SubscribeToken> listenTokens)
        {
            if (listenTokens == null)
                throw new ArgumentNullException(nameof(listenTokens));

            foreach (var listenToken in listenTokens)
                Unsubscribe(listenToken);
        }

        /// <summary>
        ///     Removes all subscriptions by their token.
        /// </summary>
        /// <param name="listenTokens">A collection of tokens to remove.</param>
        /// <exception cref="ArgumentNullException">listenTokens is null.</exception>
        public static void Unsubscribe(params SubscribeToken[] listenTokens)
        {
            if (listenTokens == null)
                throw new ArgumentNullException(nameof(listenTokens));

            foreach (var listenToken in listenTokens)
                Unsubscribe(listenToken);
        }

        private static Message<T> GetOrCreateMessage<T>()
        {
            var messageContainer = GetMessage<T>();
            if (messageContainer != null)
                return messageContainer;

            messageContainer = new Message<T>();
            _messages.Add(messageContainer);

            return messageContainer;
        }

        private static Message<T> GetMessage<T>()
        {
            return _messages.FirstOrDefault(m => m.Key == typeof(T)) as Message<T>;
        }

        private static void RemoveDead()
        {
            _messages.RemoveAll(m => !m.IsAlive);
        }
    }
}
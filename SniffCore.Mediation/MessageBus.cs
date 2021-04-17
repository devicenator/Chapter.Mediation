using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Mediation
{
    public static class MessageBus
    {
        private static readonly List<IMessage> _messages;

        static MessageBus()
        {
            _messages = new List<IMessage>();
        }

        public static SubscribeToken Subscribe<T>(Action<T> callback)
        {
            var messageContainer = (Message<T>) GetOrCreateMessage<T>();
            return messageContainer.Add(callback);
        }

        public static void Notify<T>(T implementation)
        {
            RemoveDead();
            var messageContainer = (Message<T>) GetMessage<T>();
            messageContainer?.Send(implementation);
        }

        public static void Unsubscribe(SubscribeToken subscribeToken)
        {
            foreach (var message in _messages)
                message.Remove(subscribeToken);
            RemoveDead();
        }

        public static void Unsubscribe(IEnumerable<SubscribeToken> listenTokens)
        {
            foreach (var listenToken in listenTokens)
                Unsubscribe(listenToken);
        }

        private static IMessage GetOrCreateMessage<T>()
        {
            var messageContainer = GetMessage<T>();
            if (messageContainer == null)
            {
                messageContainer = new Message<T>();
                _messages.Add(messageContainer);
            }

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
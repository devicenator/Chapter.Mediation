using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace SniffCore.Mediation.Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        [Test]
        public void Subscribe_CalledWithNullCallback_ThrowsException()
        {
            Action<string> callback = null;

            var action = new TestDelegate(() => MessageBus.Subscribe(callback));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Subscribe_CalledWithCallback_GetsNotification()
        {
            var triggered = false;
            Action<string> callback = s => triggered = true;

            MessageBus.Subscribe(callback);

            MessageBus.Notify("message");
            Assert.That(triggered, Is.True);
        }

        [Test]
        public void Subscribe_CalledWithCallbackForOtherType_GetsNoNotification()
        {
            var triggered = false;
            Action<string> callback = s => triggered = true;

            MessageBus.Subscribe(callback);

            MessageBus.Notify(13);
            Assert.That(triggered, Is.False);
        }

        [Test]
        public void Notify_CalledWithNullMessage_ThrowsException()
        {
            var action = new TestDelegate(() => MessageBus.Notify<string>(null));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Unsubscribe_CalledWithNullToken_ThrowsException()
        {
            SubscribeToken token = null;

            var action = new TestDelegate(() => MessageBus.Unsubscribe(token));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Unsubscribe_CalledWithToken_RemovesIt()
        {
            var token = MessageBus.Subscribe<string>(s =>
            {
                Assert.Fail("Callback not expected");
            });
            
            MessageBus.Unsubscribe(token);

            MessageBus.Notify("Peter");
        }

        [Test]
        public void Unsubscribe_CalledWithNullList_ThrowsException()
        {
            List<SubscribeToken> tokens = null;

            var action = new TestDelegate(() => MessageBus.Unsubscribe(tokens));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Unsubscribe_CalledWithTokenList_RemovesThem()
        {
            var tokens = new List<SubscribeToken>
            {
                MessageBus.Subscribe<string>(s => { Assert.Fail("Callback not expected"); }),
                MessageBus.Subscribe<int>(s => { Assert.Fail("Callback not expected"); })
            };

            MessageBus.Unsubscribe(tokens);

            MessageBus.Notify("Peter");
            MessageBus.Notify(13);
        }

        [Test]
        public void Unsubscribe_CalledWithNullArray_ThrowsException()
        {
            SubscribeToken[] tokens = null;

            var action = new TestDelegate(() => MessageBus.Unsubscribe(tokens));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Unsubscribe_CalledWithTokenParams_RemovesThem()
        {
            var token1 = MessageBus.Subscribe<string>(s => { Assert.Fail("Callback not expected"); });
            var token2 = MessageBus.Subscribe<int>(s => { Assert.Fail("Callback not expected"); });

            MessageBus.Unsubscribe(token1, token2);

            MessageBus.Notify("Peter");
            MessageBus.Notify(13);
        }
    }
}
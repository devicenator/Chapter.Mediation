using System;
using System.Windows.Threading;
using NUnit.Framework;

namespace SniffCore.Mediation.Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        [SetUp]
        public void Setup()
        {
            _target = new MessageBus();
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        private MessageBus _target;
        private Dispatcher _dispatcher;

        [Test]
        public void Subscribe_WithNullCallback_RaisesException()
        {
            var action = new TestDelegate(() => _target.Subscribe<string>(null));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void Subscribe_OnString_GetsCalledOnNotify()
        {
            var triggered = string.Empty;
            void Callback(string parameter)
            {
                triggered = parameter;
            }

            _target.Subscribe<string>(Callback);

            _target.Publish("Hans");
            Assert.That(triggered, Is.EqualTo("Hans"));
        }

        [Test]
        public void Subscribe_OnStringAndInt_GetsAllCalledOnNotify()
        {
            var triggeredString = string.Empty;
            void CallbackString(string parameter)
            {
                triggeredString = parameter;
            }
            var triggeredint = 0;
            void CallbackInt(int parameter)
            {
                triggeredint = parameter;
            }

            _target.Subscribe<string>(CallbackString);
            _target.Subscribe<int>(CallbackInt);
            
            _target.Publish("Hans");
            _target.Publish(13);

            Assert.That(triggeredString, Is.EqualTo("Hans"));
            Assert.That(triggeredint, Is.EqualTo(13));
        }

        [Test]
        public void Subscribe_OnStringOnDispatcher_GetsCalledOnNotify()
        {
            var triggered = string.Empty;
            void Callback(string parameter)
            {
                triggered = parameter;
            }

            _target.Subscribe<string>(Callback).On(_dispatcher);

            _target.Publish("Hans");
            Assert.That(triggered, Is.EqualTo("Hans"));
        }

        [Test]
        public void Subscribe_OnStringButAfterDispose_DoesNotNotifyAnymore()
        {
            var triggered = string.Empty;
            void Callback(string parameter)
            {
                triggered = parameter;
            }
            var subscriber = _target.Subscribe<string>(Callback);

            subscriber.Dispose();

            _target.Publish("Hans");
            Assert.That(triggered, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Subscribe_OnIntAfterStringGotDisposed_GetsCalledOnNotify()
        {
            var triggeredString = string.Empty;
            void CallbackString(string parameter)
            {
                triggeredString = parameter;
            }
            var triggeredint = 0;
            void CallbackInt(int parameter)
            {
                triggeredint = parameter;
            }
            var stringSubscriber = _target.Subscribe<string>(CallbackString);
            var intSubscriber = _target.Subscribe<int>(CallbackInt);

            stringSubscriber.Dispose();

            _target.Publish("Hans");
            _target.Publish(13);

            Assert.That(triggeredString, Is.EqualTo(string.Empty));
            Assert.That(triggeredint, Is.EqualTo(13));
        }

        [Test]
        public void Subscribe_OnStringButIntegerRaised_DoesNotGetCalled()
        {
            var triggered = string.Empty;
            void Callback(string parameter)
            {
                triggered = parameter;
            }

            _target.Subscribe<string>(Callback).On(_dispatcher);

            _target.Publish(13);
            Assert.That(triggered, Is.EqualTo(string.Empty));
        }
    }
}
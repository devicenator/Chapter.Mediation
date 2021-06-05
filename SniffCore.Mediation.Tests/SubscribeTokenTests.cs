using NUnit.Framework;

namespace SniffCore.Mediation.Tests
{
    [TestFixture]
    public class SubscribeTokenTests
    {
        [Test]
        public void Equals_CalledWithAnotherToken_ReturnsFalse()
        {
            var token1 = MessageBus.Subscribe<string>(s => { });
            var token2 = MessageBus.Subscribe<string>(s => { });

            var result = token1.Equals(token2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_CalledWithSameToken_ReturnsTrue()
        {
            var token = MessageBus.Subscribe<string>(s => { });

            var result = token.Equals(token);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_CalledWithAnotherObject_ReturnsFalse()
        {
            var token1 = MessageBus.Subscribe<string>(s => { });
            object other = "Peter";

            var result = token1.Equals(other);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_CalledWithSameObject_ReturnsTrue()
        {
            var token = MessageBus.Subscribe<string>(s => { });
            var other = (object) token;

            var result = token.Equals(other);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetHashCode_ComparedToAnotherToken_ReturnsFalse()
        {
            var token1 = MessageBus.Subscribe<string>(s => { });
            var token2 = MessageBus.Subscribe<string>(s => { });

            var result1 = token1.GetHashCode();
            var result2 = token2.GetHashCode();

            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        [Test]
        public void GetHashCode_ComparedToSameSameToken_ReturnsTrue()
        {
            var token = MessageBus.Subscribe<string>(s => { });

            var result1 = token.GetHashCode();
            var result2 = token.GetHashCode();

            Assert.That(result1, Is.EqualTo(result2));
        }
    }
}
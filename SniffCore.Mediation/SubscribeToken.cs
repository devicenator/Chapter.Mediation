using System;

namespace SniffCore.Mediation
{
    public sealed class SubscribeToken : IEquatable<SubscribeToken>
    {
        private Guid _guid;

        internal SubscribeToken()
        {
            _guid = Guid.NewGuid();
        }

        public bool Equals(SubscribeToken other)
        {
            return _guid.Equals(other._guid);
        }
    }
}
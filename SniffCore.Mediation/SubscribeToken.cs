//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

namespace SniffCore.Mediation
{
    /// <summary>
    ///     Represents a single subscriptions to a message using the <see cref="MessageBus" />.
    /// </summary>
    public sealed class SubscribeToken : IEquatable<SubscribeToken>
    {
        private readonly Guid _guid;

        internal SubscribeToken()
        {
            _guid = Guid.NewGuid();
        }

        /// <summary>
        ///     Compares this token to another token.
        /// </summary>
        /// <param name="other">The other token to compare.</param>
        /// <returns>True if the token equals; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">other is null.</exception>
        public bool Equals(SubscribeToken other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return _guid.Equals(other._guid);
        }

        /// <summary>
        ///     Compares this token to another object.
        /// </summary>
        /// <param name="obj">The other object to compare.</param>
        /// <returns>True if the token equals; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SubscribeToken other)
                return Equals(other);
            return false;
        }

        /// <summary>
        ///     Returns the hash code of this token.
        /// </summary>
        /// <returns>The hash code of this token.</returns>
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
    }
}
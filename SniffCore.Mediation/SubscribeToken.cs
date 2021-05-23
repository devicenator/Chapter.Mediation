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
        private Guid _guid;

        internal SubscribeToken()
        {
            _guid = Guid.NewGuid();
        }

        /// <summary>
        ///     Compares this token to another token.
        /// </summary>
        /// <param name="other">The other token to compare.</param>
        /// <returns>True if the token equals; otherwise false.</returns>
        public bool Equals(SubscribeToken other)
        {
            return _guid.Equals(other._guid);
        }
    }
}
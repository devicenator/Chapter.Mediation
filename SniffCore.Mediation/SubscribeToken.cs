//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

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
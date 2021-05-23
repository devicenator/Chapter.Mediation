//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

namespace SniffCore.Mediation
{
    internal interface IMessage
    {
        public Type Key { get; }
        bool IsAlive { get; }
        void Remove(SubscribeToken subscribeToken);
    }
}
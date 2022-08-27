// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;

namespace Chapter.Mediation;

/// <summary>
///     The scheduler where the <see cref="MessageBus" /> messages can be send over.
/// </summary>
public interface IScheduler
{
    /// <summary>
    ///     Invokes the action.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    void Invoke(Action action);
}
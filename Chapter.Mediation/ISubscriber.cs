﻿// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;

namespace Chapter.Mediation;

/// <summary>
///     Represents a single subscription on an object type.
/// </summary>
public interface ISubscriber : IDisposable
{
    /// <summary>
    ///     Gets raised if this subscriber got disposed.
    /// </summary>
    public event EventHandler Disposed;

    /// <summary>
    ///     Enables the dispatcher the callback gets invoked on.
    /// </summary>
    /// <param name="scheduler">The scheduler the callback gets invoked on.</param>
    /// <returns>The subscriber.</returns>
    ISubscriber On(IScheduler scheduler);
}
// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;

namespace Chapter.Mediation;

/// <summary>
///     Brings possibility to communicate between modules not knowing each other.
/// </summary>
/// <example>
///     <code lang="csharp">
/// <![CDATA[
/// public class Bootstrapper
/// {
///     public void InitContainer()
///     {
///         _unityContainer.RegisterSingleton<IMessageBus, MessageBus>();
///     }
/// }
/// 
/// public class ViewModel : ObservableObject, IDisposable
/// {
///     private List<ISubscriber> _subscribers;
/// 
///     public ViewModel(IMessageBus messageBus)
///     {
///         _subscribers = new List<ISubscriber>
///         {
///             messageBus.Subscribe<string>(OnStringReceived),
///             messageBus.Subscribe<int>(OnIntReceived).On(new Scheduler(Dispatcher.CurrentDispatcher))
///         };
///     }
/// 
///     private void OnStringReceived(string parameter)
///     {
///     }
/// 
///     private void OnIntReceived(int parameter)
///     {
///     }
/// 
///     public void Dispose()
///     {
///         _subscribers.ForEach(x => x.Dispose());
///     }
/// }
/// 
/// public class OtherViewModel : ObservableObject
/// {
///     private IMessageBus _messageBus;
/// 
///     public ViewModel(IMessageBus messageBus)
///     {
///         _messageBus = messageBus;
///     }
/// 
///     public void Publish()
///     {
///         _target.Publish("Steve");
///         _target.Publish(13);
///     }
/// }
/// ]]>
/// </code>
/// </example>
public interface IMessageBus
{
    /// <summary>
    ///     Subscribes on a specific object type.
    /// </summary>
    /// <typeparam name="T">The type of the object to subscribe on.</typeparam>
    /// <param name="callback">The callback.</param>
    /// <returns>The subscriber to dispose.</returns>
    ISubscriber Subscribe<T>(Action<T> callback);

    /// <summary>
    ///     Publishes the object all subscribers shall receive.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="item">The object instance.</param>
    void Publish<T>(T item);
}
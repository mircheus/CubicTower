using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, SubscribersList<IGlobalSubscriber>> Subscribers = new();
        
        public static Dictionary<Type, SubscribersList<IGlobalSubscriber>> SubscribersList => Subscribers;

        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            foreach (Type type in subscriberTypes)
            {
                Subscribers.TryAdd(type, new SubscribersList<IGlobalSubscriber>());
                Subscribers[type].Add(subscriber);
            }
        }

        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            foreach (Type type in subscriberTypes)
            {
                if (Subscribers.TryGetValue(type, out SubscribersList<IGlobalSubscriber> subscribersList))
                    subscribersList.Remove(subscriber);
            }
        }

        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : class, IGlobalSubscriber
        {
            if(Subscribers.ContainsKey(typeof(TSubscriber)) == false)
                return;
            
            SubscribersList<IGlobalSubscriber> subscribers = Subscribers[typeof(TSubscriber)];

            subscribers.Executing = true;
            foreach (IGlobalSubscriber subscriber in subscribers.List)
            {
                action?.Invoke(subscriber as TSubscriber);
            }

            subscribers.Executing = false;
            subscribers.Cleanup();
        }
    }
}
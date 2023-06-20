
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventBusModule
{
    public static class EventBus
    {
        private static Dictionary<Type, List<IGlobalSubscriber>> _subscribers
            = new Dictionary<Type, List<IGlobalSubscriber>>();

        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
            foreach (Type t in subscriberTypes)
            {
                if (!_subscribers.ContainsKey(t))
                    _subscribers[t] = new List<IGlobalSubscriber>();
                _subscribers[t].Add(subscriber);
            }
        }

        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
            foreach (Type t in subscriberTypes)
            {
                if (_subscribers.ContainsKey(t))
                    _subscribers[t].Remove(subscriber);
            }
        }

        public static List<Type> GetSubscriberTypes(IGlobalSubscriber globalSubscriber)
        {
            Type type = globalSubscriber.GetType();
            List<Type> subscriberTypes = type
                .GetInterfaces()
                .Where(t => t.GetInterfaces()
                    .Contains(typeof(IGlobalSubscriber)))
                .ToList();
            return subscriberTypes;
        }

        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : IGlobalSubscriber
        {
            List<IGlobalSubscriber> subscribers = _subscribers[typeof(TSubscriber)];
            foreach (IGlobalSubscriber subscriber in subscribers)
            {
                try
                {
                    action.Invoke((TSubscriber)subscriber);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}